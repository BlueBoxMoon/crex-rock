using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Web.UI.Controls;

namespace com.blueboxmoon.Crex.Web.UI.Controls
{
    public class FileTypeAndImageUploader : CompositeControl, IRockControl, IDisplayRequiredIndicator
    {
        #region IRockControl implementation (Custom implementation)

        /// <summary>
        /// Gets or sets the label text.
        /// </summary>
        /// <value>
        /// The label text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The text for the label." )
        ]
        public string Label
        {
            get { return ViewState["Label"] as string ?? string.Empty; }
            set { ViewState["Label"] = value; }
        }

        /// <summary>
        /// Gets or sets the form group class.
        /// </summary>
        /// <value>
        /// The form group class.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        Description( "The CSS class to add to the form-group div." )
        ]
        public string FormGroupCssClass
        {
            get { return ViewState["FormGroupCssClass"] as string ?? string.Empty; }
            set { ViewState["FormGroupCssClass"] = value; }
        }

        /// <summary>
        /// Gets or sets the help text.
        /// </summary>
        /// <value>
        /// The help text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The help block." )
        ]
        public string Help
        {
            get
            {
                return HelpBlock != null ? HelpBlock.Text : string.Empty;
            }

            set
            {
                if ( HelpBlock != null )
                {
                    HelpBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the warning text.
        /// </summary>
        /// <value>
        /// The warning text.
        /// </value>
        [
        Bindable( true ),
        Category( "Appearance" ),
        DefaultValue( "" ),
        Description( "The warning block." )
        ]
        public string Warning
        {
            get
            {
                return WarningBlock != null ? WarningBlock.Text : string.Empty;
            }

            set
            {
                if ( WarningBlock != null )
                {
                    WarningBlock.Text = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RockTextBox"/> is required.
        /// </summary>
        /// <value>
        ///   <c>true</c> if required; otherwise, <c>false</c>.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "false" ),
        Description( "Is the value required?" )
        ]
        public bool Required
        {
            get
            {
                EnsureChildControls();
                return _bfFileType.Required;
            }
            set
            {
                EnsureChildControls();
                _bfFileType.Required = value;
                _imgUploader.Required = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Required indicator when Required=true
        /// </summary>
        /// <value>
        /// <c>true</c> if [display required indicator]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayRequiredIndicator
        {
            get
            {
                // NOTE: Always return false since we only want the child controls (GroupType and Group dropdowns) to show the required indicator
                return false;
            }
            set
            {
                // Ignore since we always return false
            }
        }

        /// <summary>
        /// Gets or sets the required error message.  If blank, the LabelName name will be used
        /// </summary>
        /// <value>
        /// The required error message.
        /// </value>
        public string RequiredErrorMessage
        {
            get
            {
                return RequiredFieldValidator != null ? RequiredFieldValidator.ErrorMessage : string.Empty;
            }

            set
            {
                if ( RequiredFieldValidator != null )
                {
                    RequiredFieldValidator.ErrorMessage = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets an optional validation group to use.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get { return ViewState["ValidationGroup"] as string; }
            set { ViewState["ValidationGroup"] = value; this.RequiredFieldValidator.ValidationGroup = value; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsValid
        {
            get
            {
                EnsureChildControls();
                return !Required || _imgUploader.BinaryFileId.HasValue;
            }
        }

        /// <summary>
        /// Gets or sets the help block.
        /// </summary>
        /// <value>
        /// The help block.
        /// </value>
        public HelpBlock HelpBlock { get; set; }

        /// <summary>
        /// Gets or sets the warning block.
        /// </summary>
        /// <value>
        /// The warning block.
        /// </value>
        public WarningBlock WarningBlock { get; set; }

        /// <summary>
        /// Gets or sets the required field validator.
        /// </summary>
        /// <value>
        /// The required field validator.
        /// </value>
        public RequiredFieldValidator RequiredFieldValidator { get; set; }

        #endregion

        #region Controls

        private BinaryFileTypePicker _bfFileType;
        private ImageUploader _imgUploader;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the binary file identifier.
        /// </summary>
        /// <value>
        /// The binary file identifier.
        /// </value>
        public int? BinaryFileId
        {
            get
            {
                EnsureChildControls();
                return _imgUploader.BinaryFileId;
            }

            set
            {
                EnsureChildControls();

                BinaryFile binaryFile = null;

                if ( value.HasValue )
                {
                    binaryFile = new BinaryFileService( new RockContext() ).Get( value.Value );
                }

                if ( binaryFile != null )
                {
                    _bfFileType.SetValue( binaryFile.BinaryFileTypeId );
                    _imgUploader.BinaryFileTypeGuid = binaryFile.BinaryFileType.Guid;
                    _imgUploader.BinaryFileId = binaryFile.Id;
                }
                else
                {
                    _bfFileType.SetValue( ( int? ) null );
                    _imgUploader.BinaryFileId = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets the image control label.
        /// </summary>
        /// <value>
        /// The image control label.
        /// </value>
        public string ImageControlLabel
        {
            get
            {
                return ( ViewState["ImageControlLabel"] as string ) ?? "Image";
            }

            set
            {
                ViewState["ImageControlLabel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the control CSS class.
        /// </summary>
        /// <value>
        /// The control CSS class.
        /// </value>
        public string ControlCssClass
        {
            get
            {
                return ( ViewState["ControlCssClass"] as string ) ?? "col-md-12";
            }

            set
            {
                ViewState["ControlCssClass"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the thumbnail width.
        /// </summary>
        /// <value>
        /// The width of the thumbnail.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "" ),
        Description( "The optional width of the thumbnail" )
        ]
        public int ThumbnailWidth
        {
            get
            {
                return ViewState["ThumbnailWidth"] as int? ?? 100;
            }

            set
            {
                ViewState["ThumbnailWidth"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the thumbnail height.
        /// </summary>
        /// <value>
        /// The height of the thumbnail.
        /// </value>
        [
        Bindable( true ),
        Category( "Behavior" ),
        DefaultValue( "" ),
        Description( "The optional height of the thumbnail" )
        ]
        public int ThumbnailHeight
        {
            get
            {
                return ViewState["ThumbnailHeight"] as int? ?? 100;
            }

            set
            {
                ViewState["ThumbnailHeight"] = value;
            }
        }
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="FileTypeAndImageUploader"/> class.
        /// </summary>
        public FileTypeAndImageUploader()
            : base()
        {
            HelpBlock = new HelpBlock();
            WarningBlock = new WarningBlock();
            RequiredFieldValidator = new RequiredFieldValidator
            {
                ValidationGroup = this.ValidationGroup
            };
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            Controls.Clear();

            _bfFileType = new BinaryFileTypePicker();
            _imgUploader = new ImageUploader();

            RockControlHelper.CreateChildControls( this, Controls );

            _bfFileType.ID = this.ID + "_bfFileType";
            _bfFileType.AutoPostBack = true;
            _bfFileType.Label = "File Type";
            _bfFileType.SelectedIndexChanged += bfFileType_SelectedIndexChanged;
            Controls.Add( _bfFileType );

            _imgUploader.ID = this.ID + "_imgUploader";
            Controls.Add( _imgUploader );
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the _bfFileType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void bfFileType_SelectedIndexChanged( object sender, EventArgs e )
        {
            int? fileTypeId = _bfFileType.SelectedValue.AsIntegerOrNull();
            var binaryFileType = new BinaryFileTypeService( new RockContext() ).Get( fileTypeId ?? 0 );

            if ( binaryFileType != null )
            {
                if ( _imgUploader.BinaryFileTypeGuid != binaryFileType.Guid )
                {
                    _imgUploader.BinaryFileId = null;
                }
                _imgUploader.BinaryFileTypeGuid = binaryFileType.Guid;
            }
            else
            {
                _imgUploader.BinaryFileId = null;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender( EventArgs e )
        {
            base.OnPreRender( e );

            _imgUploader.Visible = _bfFileType.SelectedValueAsId().HasValue;
            _imgUploader.Label = this.ImageControlLabel;
            _imgUploader.ThumbnailWidth = this.ThumbnailWidth;
            _imgUploader.ThumbnailHeight = this.ThumbnailHeight;
            this.RequiredFieldValidator.Visible = false;
        }

        /// <summary>
        /// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( this.Visible )
            {
                RockControlHelper.RenderControl( this, writer );
            }
        }

        /// <summary>
        /// Renders the base control.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public void RenderBaseControl( HtmlTextWriter writer )
        {
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, ControlCssClass );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    _bfFileType.RenderControl( writer );
                }
                writer.RenderEndTag();

                writer.AddAttribute( HtmlTextWriterAttribute.Class, ControlCssClass );
                writer.RenderBeginTag( HtmlTextWriterTag.Div );
                {
                    _imgUploader.RenderControl( writer );
                }
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
    }
}
