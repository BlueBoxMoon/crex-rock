using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using com.blueboxmoon.Crex;
using com.blueboxmoon.Crex.Rest;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Security;
using Rock.Web.UI.Controls;
using Rock.Field.Types;

namespace RockWeb.Plugins.com_blueboxmoon.Crex
{
    [DisplayName( "Crex Content Channel View" )]
    [Category( "Blue Box Moon > Crex" )]
    [Description( "Displays a list of content channel items." )]
    [TextField( "Layout", "The screen layout to use on the TV.", true, "PosterList", "CustomSetting" )]
    [TextField( "Content Channel", "The content channel whose items will be displayed.", true, "", "CustomSetting" )]
    [TextField( "Detail Page", "The page to navigate to for details.", false, "", "CustomSetting" )]
    [TextField( "Status", "Include items with the following status.", false, "2", "CustomSetting" )]
    [IntegerField( "Filter Id", "The data filter that is used to filter items", false, 0, "CustomSetting" )]
    [TextField( "Order", "The specifics of how items should be ordered. This value is set through configuration and should not be modified here.", false, "", "CustomSetting" )]
    [TextField( "Background Image", "The image to be displayed in the background. Suggested size is 3840x2160.", false, "", "CustomSetting" )]
    [TextField( "Background Image Url", "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>", false, "", order: 2 )]
    [TextField( "Template", "The lava template to use when generating the items.", true, "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PosterListContentChannel.lava' %}", "CustomSetting" )]
    [TextField( "Enabled Lava Commands", "The lava commands to make available when parsing the template.", false, "", order: 0 )]
    public partial class CrexContentChannelView : CrexBlockCustomSettings
    {
        #region Properties

        /// <summary>
        /// Gets the preview panel.
        /// </summary>
        /// <value>
        /// The preview panel.
        /// </value>
        protected override Panel PreviewPanel { get { return tPreview; } }

        /// <summary>
        /// Gets the debug panel.
        /// </summary>
        /// <value>
        /// The debug panel.
        /// </value>
        protected override Panel DebugPanel { get { return tDebug; } }

        #endregion

        #region Base Method Overrides

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            var channelGuid = ViewState["ChannelGuid"] as Guid?;

            var rockContext = new RockContext();

            var channel = new ContentChannelService( rockContext ).Queryable( "ContentChannelType" )
                .FirstOrDefault( c => c.Guid.Equals( channelGuid.Value ) );
            if ( channel != null )
            {
                CreateFilterControl( channel, DataViewFilter.FromJson( ViewState["DataViewFilter"].ToString() ), false, rockContext );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:Init" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            BlockUpdated += CrexContentChannelView_BlockUpdated;
        }

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            ViewState["ChannelGuid"] = ddlChannel.SelectedValue.AsGuidOrNull();
            ViewState["DataViewFilter"] = GetFilterControl().ToJson();

            return base.SaveViewState();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the CrexContentChannelView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrexContentChannelView_BlockUpdated( object sender, EventArgs e )
        {
            ShowPreview();
        }

        /// <summary>
        /// Raises the <see cref="E:Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            nbError.Text = string.Empty;

            if ( !IsPostBack )
            {
                ShowPreview();
            }
        }

        /// <summary>
        /// Shows the settings.
        /// </summary>
        protected override void ShowSettings()
        {
            // Switch does not automatically initialize again after a partial-postback.  This script 
            // looks for any switch elements that have not been initialized and re-intializes them.
            string script = @"
;$(document).ready(function() {
    $('.switch > input').each( function () {
        $(this).parent().switch('init');
    });
});
";
            ScriptManager.RegisterStartupScript( this.Page, this.Page.GetType(), "toggle-switch-init", script, true );

            mdEdit.Show();

            var rockContext = new RockContext();
            ddlChannel.DataSource = new ContentChannelService( rockContext ).Queryable()
                .OrderBy( c => c.Name )
                .Select( c => new { c.Guid, c.Name } )
                .ToList();
            ddlChannel.DataBind();
            ddlChannel.Items.Insert( 0, new ListItem( "", "" ) );
            ddlChannel.SetValue( GetAttributeValue( "ContentChannel" ) );

            cblStatus.BindToEnum<ContentChannelItemStatus>();
            foreach ( string status in GetAttributeValue( "Status" ).SplitDelimitedValues() )
            {
                var li = cblStatus.Items.FindByValue( status );
                if ( li != null )
                {
                    li.Selected = true;
                }
            }

            ddlLayout.SetValue( GetAttributeValue( "Layout" ) );

            var ppFieldType = new PageReferenceFieldType();
            ppFieldType.SetEditValue( ppDetailPage, null, GetAttributeValue( "DetailPage" ) );

            hfDataFilterId.Value = GetAttributeValue( "FilterId" );

            var directions = new Dictionary<string, string>();
            directions.Add( "", "" );
            directions.Add( SortDirection.Ascending.ConvertToInt().ToString(), "Ascending" );
            directions.Add( SortDirection.Descending.ConvertToInt().ToString(), "Descending" );
            kvlOrder.CustomValues = directions;
            kvlOrder.Value = GetAttributeValue( "Order" );
            kvlOrder.Required = true;

            var binaryFile = new BinaryFileService( rockContext ).Get( GetAttributeValue( "BackgroundImage" ).AsGuid() );
            ftBackgroundImage.BinaryFileId = binaryFile != null ? ( int? ) binaryFile.Id : null;
            tbBackgroundImageUrl.Text = GetAttributeValue( "BackgroundImageUrl" );

            ceTemplate.Text = GetAttributeValue( "Template" );

            ShowEdit( ddlChannel.SelectedValue.AsGuidOrNull() );
        }

        /// <summary>
        /// Shows the error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected override void ShowError( string message )
        {
            nbError.Text = message;
        }

        /// <summary>
        /// Gets the data that identifies this Crex template and action data.
        /// </summary>
        /// <param name="isPreview">if set to <c>true</c> then this is for a preview.</param>
        /// <returns>A CrexAction object.</returns>
        public override CrexAction GetCrexAction( bool isPreview )
        {
            var mergeFields = GetCommonMergeFields();
            var layout = GetAttributeValue( "Layout" );
            var lavaTemplate = GetAttributeValue( "Template" );

            using ( var rockContext = new RockContext() )
            {
                var contentChannel = new ContentChannelService( rockContext ).Get( GetAttributeValue( "ContentChannel" ).AsGuid() );
                var errorMessages = new List<string>();
                var contentChannelItemService = new ContentChannelItemService( rockContext );
                var contentChannelItems = GetContent( errorMessages );

                mergeFields.AddOrReplace( "ContentChannel", contentChannel );
                mergeFields.AddOrReplace( "Items", contentChannelItems.ToList() );
                var detailPage = Rock.Web.Cache.PageCache.Read( GetAttributeValue( "DetailPage" ).AsGuid() );
                var linkedPages = new Dictionary<string, object>
                {
                    { "DetailPageId", detailPage != null ? detailPage.Id.ToString() : string.Empty }
                };
                mergeFields.Add( "LinkedPages", linkedPages );

                var json = lavaTemplate.ResolveMergeFields( mergeFields, CurrentPerson, GetAttributeValue( "EnabledLavaCommands" ) );

                if ( layout == "Menu" )
                {
                    var menuData = new com.blueboxmoon.Crex.Rest.Menu
                    {
                        BackgroundImage = GetUrlSetFromAttributes( "BackgroundImage", "BackgroundImageUrl", mergeFields )
                    };

                    menuData.Buttons = json.FromJsonOrNull<List<MenuButton>>();

                    return new CrexAction( layout, menuData );
                }
                else if ( layout == "PosterList" )
                {
                    var posterListData = new PosterList
                    {
                        BackgroundImage = GetUrlSetFromAttributes( "BackgroundImage", "BackgroundImageUrl", mergeFields ),
                        Title = PageCache.PageTitle
                    };

                    posterListData.Items = json.FromJsonOrNull<List<PosterListItem>>();

                    return new CrexAction( layout, posterListData );
                }
            }

            return new CrexAction();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shows the edit.
        /// </summary>
        public void ShowEdit( Guid? channelGuid )
        {
            int? filterId = hfDataFilterId.Value.AsIntegerOrNull();

            if ( channelGuid.HasValue )
            {
                var rockContext = new RockContext();
                var channel = new ContentChannelService( rockContext ).Queryable( "ContentChannelType" )
                    .FirstOrDefault( c => c.Guid.Equals( channelGuid.Value ) );
                if ( channel != null )
                {
                    cblStatus.Visible = channel.RequiresApproval && !channel.ContentChannelType.DisableStatus;

                    var filterService = new DataViewFilterService( rockContext );
                    DataViewFilter filter = null;

                    if ( filterId.HasValue )
                    {
                        filter = filterService.Get( filterId.Value );
                    }

                    if ( filter == null || filter.ExpressionType == FilterExpressionType.Filter )
                    {
                        filter = new DataViewFilter();
                        filter.Guid = new Guid();
                        filter.ExpressionType = FilterExpressionType.GroupAll;
                    }

                    CreateFilterControl( channel, filter, true, rockContext );

                    kvlOrder.CustomKeys = new Dictionary<string, string>();
                    kvlOrder.CustomKeys.Add( "", "" );
                    kvlOrder.CustomKeys.Add( "Title", "Title" );
                    kvlOrder.CustomKeys.Add( "Priority", "Priority" );
                    kvlOrder.CustomKeys.Add( "Status", "Status" );
                    kvlOrder.CustomKeys.Add( "StartDateTime", "Start" );
                    kvlOrder.CustomKeys.Add( "ExpireDateTime", "Expire" );
                    kvlOrder.CustomKeys.Add( "Order", "Order" );

                    //
                    // Add item attributes to the custom keys.
                    //
                    AttributeService attributeService = new AttributeService( rockContext );
                    var itemAttributes = attributeService.GetByEntityTypeId( new ContentChannelItem().TypeId ).AsQueryable()
                                            .Where( a => (
                                                    a.EntityTypeQualifierColumn.Equals( "ContentChannelTypeId", StringComparison.OrdinalIgnoreCase ) &&
                                                    a.EntityTypeQualifierValue.Equals( channel.ContentChannelTypeId.ToString() )
                                                ) || (
                                                    a.EntityTypeQualifierColumn.Equals( "ContentChannelId", StringComparison.OrdinalIgnoreCase ) &&
                                                    a.EntityTypeQualifierValue.Equals( channel.Id.ToString() )
                                                ) )
                                            .OrderByDescending( a => a.EntityTypeQualifierColumn )
                                            .ThenBy( a => a.Order )
                                            .ToList();

                    foreach ( var attribute in itemAttributes )
                    {
                        string attrKey = "Attribute:" + attribute.Key;
                        if ( !kvlOrder.CustomKeys.ContainsKey( attrKey ) )
                        {
                            kvlOrder.CustomKeys.Add( attrKey, attribute.Name );
                        }
                    }

                }
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        private List<ContentChannelItem> GetContent( List<string> errorMessages )
        {
            List<ContentChannelItem> items = null;

            Guid? channelGuid = GetAttributeValue( "ContentChannel" ).AsGuidOrNull();
            if ( channelGuid.HasValue )
            {
                var rockContext = new RockContext();
                var service = new ContentChannelItemService( rockContext );
                var itemType = typeof( Rock.Model.ContentChannelItem );

                var contentChannel = new ContentChannelService( rockContext ).Get( channelGuid.Value );
                if ( contentChannel != null )
                {
                    var entityFields = HackEntityFields( contentChannel, rockContext );

                    items = new List<ContentChannelItem>();

                    var qry = service
                        .Queryable( "ContentChannel,ContentChannelType" )
                        .Where( i => i.ContentChannelId == contentChannel.Id );

                    //
                    // Filter by Status
                    //
                    if ( contentChannel.RequiresApproval && !contentChannel.ContentChannelType.DisableStatus )
                    {
                        var statuses = GetAttributeValues( "Status" )
                            .Select( s => s.ConvertToEnumOrNull<ContentChannelItemStatus>() )
                            .Where( s => s.HasValue )
                            .Select( s => s.Value );

                        if ( statuses.Any() )
                        {
                            qry = qry.Where( i => statuses.Contains( i.Status ) );
                        }
                    }

                    //
                    // Filer by user-supplied filter options.
                    //
                    int? dataFilterId = GetAttributeValue( "FilterId" ).AsIntegerOrNull();
                    if ( dataFilterId.HasValue )
                    {
                        var dataFilterService = new DataViewFilterService( rockContext );
                        var dataFilter = dataFilterService.Queryable( "ChildFilters" ).FirstOrDefault( a => a.Id == dataFilterId.Value );

                        if ( dataFilter != null )
                        {
                            var parameterExpression = service.ParameterExpression;
                            Expression whereExpression = dataFilter.GetExpression( itemType, service, parameterExpression, errorMessages );

                            qry = qry.Where( parameterExpression, whereExpression, null );
                        }
                    }

                    //
                    // All filtering has been performed, now run query and check security.
                    //
                    foreach ( var item in qry.ToList() )
                    {
                        if ( item.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
                        {
                            items.Add( item );
                        }
                    }

                    //
                    // Order the items.
                    //
                    string orderBy = GetAttributeValue( "Order" );
                    if ( !string.IsNullOrWhiteSpace( orderBy ) )
                    {
                        List<SortProperty> sortProperties = new List<SortProperty>();

                        //
                        // Convert the user-provided sorting options into a format that can be used by SortProperty.
                        //
                        var fieldDirection = new List<string>();
                        foreach ( var itemPair in orderBy.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries ).Select( a => a.Split( '^' ) ) )
                        {
                            if ( itemPair.Length == 2 && !string.IsNullOrWhiteSpace( itemPair[0] ) )
                            {
                                sortProperties.Add( new SortProperty
                                {
                                    Property = itemPair[0],
                                    Direction = itemPair[1].ConvertToEnum<SortDirection>( SortDirection.Ascending )
                                } );
                            }
                        }

                        items = OrderBy( items, sortProperties );
                    }
                }
            }

            return items;

        }

        /// <summary>
        /// Orders the list of items by the specified sort properties..
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="sortProperties">The sort properties.</param>
        /// <returns></returns>
        private static List<ContentChannelItem> OrderBy( List<ContentChannelItem> items, IEnumerable<SortProperty> sortProperties )
        {
            var itemQry = items.AsQueryable();
            IOrderedQueryable<ContentChannelItem> orderedQry = null;
            bool loadedAttributes = false;

            foreach ( var sortProperty in sortProperties )
            {
                if ( sortProperty.Property.StartsWith( "Attribute:" ) )
                {
                    //
                    // Column is an attribute to be sorted.
                    //
                    string attributeKey = sortProperty.Property.Substring( 10 ).Trim();

                    //
                    // Make sure all attributes are loaded if we haven't yet verified.
                    //
                    if ( !loadedAttributes )
                    {
                        using ( var rockContext = new RockContext() )
                        {
                            foreach ( var item in orderedQry ?? itemQry )
                            {
                                if ( item.Attributes == null )
                                {
                                    item.LoadAttributes( rockContext );
                                }
                            }
                        }

                        loadedAttributes = true;
                    }

                    //
                    // Perform sort by the attribute value.
                    //
                    if ( sortProperty.Direction == SortDirection.Ascending )
                    {
                        orderedQry = ( orderedQry == null ) ?
                            itemQry.OrderBy( i => i.AttributeValues.Where( v => v.Key == attributeKey ).FirstOrDefault().Value.SortValue ) :
                            orderedQry.ThenBy( i => i.AttributeValues.Where( v => v.Key == attributeKey ).FirstOrDefault().Value.SortValue );
                    }
                    else
                    {
                        orderedQry = ( orderedQry == null ) ?
                            itemQry.OrderByDescending( i => i.AttributeValues.Where( v => v.Key == attributeKey ).FirstOrDefault().Value.SortValue ) :
                            orderedQry.ThenByDescending( i => i.AttributeValues.Where( v => v.Key == attributeKey ).FirstOrDefault().Value.SortValue );
                    }
                }
                else
                {
                    //
                    // Column is a property to be sorted.
                    //
                    if ( sortProperty.Direction == SortDirection.Ascending )
                    {
                        orderedQry = ( orderedQry == null ) ? itemQry.OrderBy( sortProperty.Property ) : orderedQry.ThenBy( sortProperty.Property );
                    }
                    else
                    {
                        orderedQry = ( orderedQry == null ) ? itemQry.OrderByDescending( sortProperty.Property ) : orderedQry.ThenByDescending( sortProperty.Property );
                    }
                }
            }

            return orderedQry != null ? orderedQry.ToList() : items;
        }

        #endregion

        #region Filter Methods

        /// <summary>
        /// **The PropertyFilter checks for it's property/attribute list in a cached items object before recreating 
        /// them using reflection and loading of generic attributes. Because of this, we're going to load them here
        /// and exclude some properties and add additional attributes specific to the channel type, and then save
        /// list to same cached object so that property filter lists our collection of properties/attributes
        /// instead.
        /// </summary>
        private List<Rock.Reporting.EntityField> HackEntityFields( ContentChannel channel, RockContext rockContext )
        {
            if ( channel != null )
            {
                var entityTypeCache = EntityTypeCache.Read( typeof( ContentChannelItem ) );
                if ( entityTypeCache != null )
                {
                    var entityType = entityTypeCache.GetEntityType();

                    /// See above comments on HackEntityFields** to see why we are doing this
                    HttpContext.Current.Items.Remove( Rock.Reporting.EntityHelper.GetCacheKey( entityType ) );

                    var entityFields = Rock.Reporting.EntityHelper.GetEntityFields( entityType );
                    foreach ( var entityField in entityFields
                        .Where( f =>
                            f.FieldKind == Rock.Reporting.FieldKind.Attribute &&
                            f.AttributeGuid.HasValue )
                        .ToList() )
                    {
                        // remove EntityFields that aren't attributes for this ContentChannelType or ChannelChannel (to avoid duplicate Attribute Keys)
                        var attribute = AttributeCache.Read( entityField.AttributeGuid.Value );
                        if ( attribute != null &&
                            attribute.EntityTypeQualifierColumn == "ContentChannelTypeId" &&
                            attribute.EntityTypeQualifierValue.AsInteger() != channel.ContentChannelTypeId )
                        {
                            entityFields.Remove( entityField );
                        }

                        if ( attribute != null &&
                            attribute.EntityTypeQualifierColumn == "ContentChannelId" &&
                            attribute.EntityTypeQualifierValue.AsInteger() != channel.Id )
                        {
                            entityFields.Remove( entityField );
                        }
                    }

                    if ( entityFields != null )
                    {
                        // Remove the status field
                        var ignoreFields = new List<string>();
                        ignoreFields.Add( "ContentChannelId" );
                        ignoreFields.Add( "Status" );

                        entityFields = entityFields.Where( f => !ignoreFields.Contains( f.Name ) ).ToList();

                        // Add any additional attributes that are specific to channel/type
                        var item = new ContentChannelItem();
                        item.ContentChannel = channel;
                        item.ContentChannelId = channel.Id;
                        item.ContentChannelType = channel.ContentChannelType;
                        item.ContentChannelTypeId = channel.ContentChannelTypeId;
                        item.LoadAttributes( rockContext );
                        foreach ( var attribute in item.Attributes
                            .Where( a =>
                                a.Value.EntityTypeQualifierColumn != "" &&
                                a.Value.EntityTypeQualifierValue != "" )
                            .Select( a => a.Value ) )
                        {
                            if ( !entityFields.Any( f => f.AttributeGuid.Equals( attribute.Guid ) ) )
                            {
                                Rock.Reporting.EntityHelper.AddEntityFieldForAttribute( entityFields, attribute );
                            }
                        }

                        // Re-sort fields
                        int index = 0;
                        var sortedFields = new List<Rock.Reporting.EntityField>();
                        foreach ( var entityProperty in entityFields.OrderBy( p => p.Title ).ThenBy( p => p.Name ) )
                        {
                            entityProperty.Index = index;
                            index++;
                            sortedFields.Add( entityProperty );
                        }

                        // Save new fields to cache ( which report field will use instead of reading them again )
                        HttpContext.Current.Items[Rock.Reporting.EntityHelper.GetCacheKey( entityType )] = sortedFields;
                    }

                    return entityFields;
                }
            }

            return null;
        }

        /// <summary>
        /// Creates the filter control.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="setSelection">if set to <c>true</c> [set selection].</param>
        /// <param name="rockContext">The rock context.</param>
        private void CreateFilterControl( ContentChannel channel, DataViewFilter filter, bool setSelection, RockContext rockContext )
        {
            HackEntityFields( channel, rockContext );

            phFilters.Controls.Clear();
            if ( filter != null )
            {
                CreateFilterControl( phFilters, filter, setSelection, rockContext );
            }
        }

        /// <summary>
        /// Creates the filter control.
        /// </summary>
        /// <param name="parentControl">The parent control.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="setSelection">if set to <c>true</c> [set selection].</param>
        /// <param name="rockContext">The rock context.</param>
        private void CreateFilterControl( Control parentControl, DataViewFilter filter, bool setSelection, RockContext rockContext )
        {
            try
            {
                if ( filter.ExpressionType == FilterExpressionType.Filter )
                {
                    var filterControl = new FilterField();

                    parentControl.Controls.Add( filterControl );
                    filterControl.DataViewFilterGuid = filter.Guid;
                    filterControl.ID = string.Format( "ff_{0}", filterControl.DataViewFilterGuid.ToString( "N" ) );

                    // Remove the 'Other Data View' Filter as it doesn't really make sense to have it available in this scenario
                    filterControl.ExcludedFilterTypes = new string[] { typeof( Rock.Reporting.DataFilter.OtherDataViewFilter ).FullName };
                    filterControl.FilteredEntityTypeName = typeof( Rock.Model.ContentChannelItem ).FullName;

                    if ( filter.EntityTypeId.HasValue )
                    {
                        var entityTypeCache = Rock.Web.Cache.EntityTypeCache.Read( filter.EntityTypeId.Value, rockContext );
                        if ( entityTypeCache != null )
                        {
                            filterControl.FilterEntityTypeName = entityTypeCache.Name;
                        }
                    }

                    filterControl.Expanded = filter.Expanded;
                    if ( setSelection )
                    {
                        try
                        {
                            filterControl.SetSelection( filter.Selection );
                        }
                        catch ( Exception ex )
                        {
                            this.LogException( new Exception( "Exception setting selection for DataViewFilter: " + filter.Guid, ex ) );
                        }
                    }

                    filterControl.DeleteClick += filterControl_DeleteClick;
                }
                else
                {
                    var groupControl = new FilterGroup();
                    parentControl.Controls.Add( groupControl );
                    groupControl.DataViewFilterGuid = filter.Guid;
                    groupControl.ID = string.Format( "fg_{0}", groupControl.DataViewFilterGuid.ToString( "N" ) );
                    groupControl.FilteredEntityTypeName = typeof( Rock.Model.ContentChannelItem ).FullName;
                    groupControl.IsDeleteEnabled = parentControl is FilterGroup;
                    if ( setSelection )
                    {
                        groupControl.FilterType = filter.ExpressionType;
                    }

                    groupControl.AddFilterClick += groupControl_AddFilterClick;
                    groupControl.AddGroupClick += groupControl_AddGroupClick;
                    groupControl.DeleteGroupClick += groupControl_DeleteGroupClick;
                    foreach ( var childFilter in filter.ChildFilters )
                    {
                        CreateFilterControl( groupControl, childFilter, setSelection, rockContext );
                    }
                }
            }
            catch ( Exception ex )
            {
                this.LogException( new Exception( "Exception creating FilterControl for DataViewFilter: " + filter.Guid, ex ) );
            }
        }

        private DataViewFilter GetFilterControl()
        {
            if ( phFilters.Controls.Count > 0 )
            {
                return GetFilterControl( phFilters.Controls[0] );
            }

            return null;
        }

        private DataViewFilter GetFilterControl( Control control )
        {
            FilterGroup groupControl = control as FilterGroup;
            if ( groupControl != null )
            {
                return GetFilterGroupControl( groupControl );
            }

            FilterField filterControl = control as FilterField;
            if ( filterControl != null )
            {
                return GetFilterFieldControl( filterControl );
            }

            return null;
        }

        private DataViewFilter GetFilterGroupControl( FilterGroup filterGroup )
        {
            DataViewFilter filter = new DataViewFilter();
            filter.Guid = filterGroup.DataViewFilterGuid;
            filter.ExpressionType = filterGroup.FilterType;
            foreach ( Control control in filterGroup.Controls )
            {
                DataViewFilter childFilter = GetFilterControl( control );
                if ( childFilter != null )
                {
                    filter.ChildFilters.Add( childFilter );
                }
            }

            return filter;
        }

        private DataViewFilter GetFilterFieldControl( FilterField filterField )
        {
            DataViewFilter filter = new DataViewFilter();
            filter.Guid = filterField.DataViewFilterGuid;
            filter.ExpressionType = FilterExpressionType.Filter;
            filter.Expanded = filterField.Expanded;
            if ( filterField.FilterEntityTypeName != null )
            {
                filter.EntityTypeId = Rock.Web.Cache.EntityTypeCache.Read( filterField.FilterEntityTypeName ).Id;
                filter.Selection = filterField.GetSelection();
            }

            return filter;
        }

        private void SetNewDataFilterGuids( DataViewFilter dataViewFilter )
        {
            if ( dataViewFilter != null )
            {
                dataViewFilter.Guid = Guid.NewGuid();
                foreach ( var childFilter in dataViewFilter.ChildFilters )
                {
                    SetNewDataFilterGuids( childFilter );
                }
            }
        }

        private void DeleteDataViewFilter( DataViewFilter dataViewFilter, DataViewFilterService service )
        {
            if ( dataViewFilter != null )
            {
                foreach ( var childFilter in dataViewFilter.ChildFilters.ToList() )
                {
                    DeleteDataViewFilter( childFilter, service );
                }

                service.Delete( dataViewFilter );
            }
        }

        #endregion

        #region Filter Events

        /// <summary>
        /// Handles the AddFilterClick event of the groupControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void groupControl_AddFilterClick( object sender, EventArgs e )
        {
            FilterGroup groupControl = sender as FilterGroup;
            FilterField filterField = new FilterField();
            filterField.DataViewFilterGuid = Guid.NewGuid();
            groupControl.Controls.Add( filterField );
            filterField.ID = string.Format( "ff_{0}", filterField.DataViewFilterGuid.ToString( "N" ) );

            // Remove the 'Other Data View' Filter as it doesn't really make sense to have it available in this scenario
            filterField.ExcludedFilterTypes = new string[] { typeof( Rock.Reporting.DataFilter.OtherDataViewFilter ).FullName };
            filterField.FilteredEntityTypeName = groupControl.FilteredEntityTypeName;
            filterField.Expanded = true;
        }

        /// <summary>
        /// Handles the AddGroupClick event of the groupControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void groupControl_AddGroupClick( object sender, EventArgs e )
        {
            FilterGroup groupControl = sender as FilterGroup;
            FilterGroup childGroupControl = new FilterGroup();
            childGroupControl.DataViewFilterGuid = Guid.NewGuid();
            groupControl.Controls.Add( childGroupControl );
            childGroupControl.ID = string.Format( "fg_{0}", childGroupControl.DataViewFilterGuid.ToString( "N" ) );
            childGroupControl.FilteredEntityTypeName = groupControl.FilteredEntityTypeName;
            childGroupControl.FilterType = FilterExpressionType.GroupAll;
        }

        /// <summary>
        /// Handles the DeleteClick event of the filterControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void filterControl_DeleteClick( object sender, EventArgs e )
        {
            FilterField fieldControl = sender as FilterField;
            fieldControl.Parent.Controls.Remove( fieldControl );
        }

        /// <summary>
        /// Handles the DeleteGroupClick event of the groupControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void groupControl_DeleteGroupClick( object sender, EventArgs e )
        {
            FilterGroup groupControl = sender as FilterGroup;
            groupControl.Parent.Controls.Remove( groupControl );
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the SaveClick event of the mdEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void mdEdit_SaveClick( object sender, EventArgs e )
        {
            var dataViewFilter = GetFilterControl();

            // update Guids since we are creating a new dataFilter and children and deleting the old one
            SetNewDataFilterGuids( dataViewFilter );

            if ( !Page.IsValid )
            {
                return;
            }

            if ( !dataViewFilter.IsValid )
            {
                // Controls will render the error messages                    
                return;
            }

            var rockContext = new RockContext();
            DataViewFilterService dataViewFilterService = new DataViewFilterService( rockContext );

            int? dataViewFilterId = hfDataFilterId.Value.AsIntegerOrNull();
            if ( dataViewFilterId.HasValue )
            {
                var oldDataViewFilter = dataViewFilterService.Get( dataViewFilterId.Value );
                DeleteDataViewFilter( oldDataViewFilter, dataViewFilterService );
            }

            dataViewFilterService.Add( dataViewFilter );

            //
            // Delete the old file and persist the new file - if they are different.
            //
            var binaryFileService = new BinaryFileService( rockContext );
            var oldBinaryFileGuid = GetAttributeValue( "BackgroundImage" ).AsGuidOrNull();
            var newBinaryFile = binaryFileService.Get( ftBackgroundImage.BinaryFileId ?? 0 );
            if ( oldBinaryFileGuid.HasValue && ( newBinaryFile == null || oldBinaryFileGuid.Value != newBinaryFile.Guid ) )
            {
                var oldBinaryFile = binaryFileService.Get( oldBinaryFileGuid.Value );
                if ( oldBinaryFile != null )
                {
                    oldBinaryFile.IsTemporary = true;
                }
            }
            if ( newBinaryFile != null && ( !oldBinaryFileGuid.HasValue || newBinaryFile.Guid != oldBinaryFileGuid.Value ) )
            {
                newBinaryFile.IsTemporary = false;
            }

            rockContext.SaveChanges();

            SetAttributeValue( "ContentChannel", ddlChannel.SelectedValue );
            SetAttributeValue( "Status", cblStatus.SelectedValuesAsInt.AsDelimited( "," ) );
            SetAttributeValue( "Layout", ddlLayout.SelectedValue );
            var ppFieldType = new PageReferenceFieldType();
            SetAttributeValue( "DetailPage", ppFieldType.GetEditValue( ppDetailPage, null ) );
            SetAttributeValue( "FilterId", dataViewFilter.Id.ToString() );
            SetAttributeValue( "Order", kvlOrder.Value );
            SetAttributeValue( "BackgroundImage", newBinaryFile != null ? newBinaryFile.Guid.ToString() : string.Empty );
            SetAttributeValue( "BackgroundImageUrl", tbBackgroundImageUrl.Text );
            SetAttributeValue( "Template", ceTemplate.Text );

            SaveAttributeValues();

            mdEdit.Hide();

            ShowPreview();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlChannel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void ddlChannel_SelectedIndexChanged( object sender, EventArgs e )
        {
            ShowEdit( ddlChannel.SelectedValue.AsGuidOrNull() );
        }

        #endregion
    }
}
