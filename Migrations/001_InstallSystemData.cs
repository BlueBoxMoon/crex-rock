using Rock.Plugin;

namespace com.blueboxmoon.Crex.Migrations
{
    [MigrationNumber( 1, "1.7.4" )]
    public class InstallSystemData : ExtendedMigration
    {
        public override void Up()
        {
            #region Block Type Crex Content Channel Item Children

            // BlockType: Crex Content Channel Item Children
            RockMigrationHelper.AddBlockType( "Crex Content Channel Item Children",
                "Displays a list of content channel items that are children of the passed content channel item.",
                "~/Plugins/com_blueboxmoon/Crex/CrexContentChannelItemChildren.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN );

            // Attribute for BlockType Crex Content Channel Item Children: Allowed Channel Types
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Allowed Channel Types",
                "AllowedChannelTypes",
                "",
                "Limits the content channel types that can be viewed.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__ALLOWED_CHANNEL_TYPES,
                false );

            // Attribute for BlockType Crex Content Channel Item Children: Background Image
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image",
                "BackgroundImage",
                "",
                "The image to be displayed in the background. Suggested size is 3840x2160.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__BACKGROUND_IMAGE,
                false );

            // Attribute for BlockType Crex Content Channel Item Children: Background Image Url
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image Url",
                "BackgroundImageUrl",
                "",
                "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__BACKGROUND_IMAGE_URL,
                false );

            // Attribute for BlockType Crex Content Channel Item Children: Detail Page
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Detail Page",
                "DetailPage",
                "",
                "The page to navigate to for details.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__DETAIL_PAGE,
                false );
            
            // Attribute for BlockType Crex Content Channel Item Children: Enabled Lava Commands
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.LAVA_COMMANDS,
                "Enabled Lava Commands",
                "EnabledLavaCommands",
                "",
                "The lava commands to make available when parsing the template.",
                6,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__ENABLED_LAVA_COMMANDS,
                false );

            // Attribute for BlockType Crex Content Channel Item Children: Layout
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Layout",
                "Layout",
                "",
                "The screen layout to use on the TV.",
                0,
                "Menu",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__LAYOUT,
                true );

            // Attribute for BlockType Crex Content Channel Item Children: Sort Items By
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Sort Items By",
                "SortItemsBy",
                "",
                "How to sort the child items.",
                0,
                "Descending",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__SORT_ITEMS_BY,
                true );

            // Attribute for BlockType Crex Content Channel Item Children: Template
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                Rock.SystemGuid.FieldType.TEXT,
                "Template",
                "Template",
                "",
                "The lava template to use when generating the items.",
                0,
                "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PosterListContentChannelChildren.lava' %}",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__TEMPLATE,
                true );

            #endregion

            #region Block Type Crex Content Channel View

            // BlockType: Crex Content Channel View
            RockMigrationHelper.AddBlockType( "Crex Content Channel View",
                "Displays a list of content channel items.",
                "~/Plugins/com_blueboxmoon/Crex/CrexContentChannelView.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW );

            // Attribute for BlockType Crex Content Channel View: Background Image
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image",
                "BackgroundImage",
                "",
                "The image to be displayed in the background. Suggested size is 3840x2160.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__BACKGROUND_IMAGE,
                false );

            // Attribute for BlockType Crex Content Channel View: Background Image Url
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image Url",
                "BackgroundImageUrl",
                "",
                "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>",
                2,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__BACKGROUND_IMAGE_URL,
                false );

            // Attribute for BlockType Crex Content Channel View: Content Channel
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Content Channel",
                "ContentChannel",
                "",
                "The content channel whose items will be displayed.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__CONTENT_CHANNEL,
                true );

            // Attribute for BlockType Crex Content Channel View: Detail Page
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Detail Page",
                "DetailPage",
                "",
                "The page to navigate to for details.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__DETAIL_PAGE,
                true );

            // Attribute for BlockType Crex Content Channel View: Enabled Lava Commands
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Enabled Lava Commands",
                "EnabledLavaCommands",
                "",
                "The lava commands to make available when parsing the template.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__ENABLED_LAVA_COMMANDS,
                false );

            // Attribute for BlockType Crex Content Channel View: Filter Id
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.INTEGER,
                "Filter Id",
                "FilterId",
                "",
                "The data filter that is used to filter items",
                0,
                "0",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__FILTER_ID,
                false );

            // Attribute for BlockType Crex Content Channel View: Layout
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Layout",
                "Layout",
                "",
                "The screen layout to use on the TV.",
                0,
                "PosterList",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__LAYOUT,
                true );

            // Attribute for BlockType Crex Content Channel View: Order
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Order",
                "Order",
                "",
                "The specifics of how items should be ordered. This value is set through configuration and should not be modified here.",
                0,
                "",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__ORDER,
                false );

            // Attribute for BlockType Crex Content Channel View: Status
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Status",
                "Status",
                "",
                "Include items with the following status.",
                0,
                "2",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__STATUS,
                false );

            // Attribute for BlockType Crex Content Channel View: Template
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                Rock.SystemGuid.FieldType.TEXT,
                "Template",
                "Template",
                "",
                "The lava template to use when generating the items.",
                0,
                "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PosterListContentChannel.lava' %}",
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__TEMPLATE,
                true );

            #endregion

            #region Block Type Crex Image

            // BlockType: Crex Image
            RockMigrationHelper.AddBlockType( "Crex Image",
                "Displays an image that can be specified with Lava.",
                "~/Plugins/com_blueboxmoon/Crex/CrexImage.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_IMAGE );

            // Attribute for BlockType Crex Image: Background Image
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_IMAGE,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image",
                "BackgroundImage",
                "",
                "The image to be displayed in the background. Suggested size is 3840x2160.",
                0,
                "",
                SystemGuid.Attribute.CREX_IMAGE__BACKGROUND_IMAGE,
                false );

            // Attribute for BlockType Crex Image: Background Image Url
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_IMAGE,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image Url",
                "BackgroundImageUrl",
                "",
                "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>",
                0,
                "",
                SystemGuid.Attribute.CREX_IMAGE__BACKGROUND_IMAGE_URL,
                false );

            // Attribute for BlockType Crex Image: Entity Type
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_IMAGE,
                Rock.SystemGuid.FieldType.ENTITYTYPE,
                "Entity Type",
                "ContextEntityType",
                "",
                "The type of entity that will provide context for this block",
                0,
                "",
                SystemGuid.Attribute.CREX_IMAGE__ENTITY_TYPE,
                false );

            #endregion

            #region Block Type Crex Lava Template

            // BlockType: Crex Lava Template
            RockMigrationHelper.AddBlockType( "Crex Lava Template",
                "Send a fully customized template to the TV application.",
                "~/Plugins/com_blueboxmoon/Crex/CrexLavaTemplate.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_LAVA_TEMPLATE );

            // Attribute for BlockType Crex Lava Template: Enabled Lava Commands
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_LAVA_TEMPLATE,
                Rock.SystemGuid.FieldType.LAVA_COMMANDS,
                "Enabled Lava Commands",
                "EnabledLavaCommands",
                "",
                "The lava commands to make available when parsing the template.",
                1,
                "",
                SystemGuid.Attribute.CREX_LAVA_TEMPLATE__ENABLED_LAVA_COMMANDS,
                false );

            // Attribute for BlockType Crex Lava Template: Entity Type
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_LAVA_TEMPLATE,
                Rock.SystemGuid.FieldType.ENTITYTYPE,
                "Entity Type",
                "ContextEntityType",
                "",
                "The type of entity that will provide context for this block",
                0,
                "",
                SystemGuid.Attribute.CREX_LAVA_TEMPLATE__ENTITY_TYPE,
                false );

            // Attribute for BlockType Crex Lava Template: Template
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_LAVA_TEMPLATE,
                Rock.SystemGuid.FieldType.CODE_EDITOR,
                "Template",
                "Template",
                "",
                "See the documentation for the the exact syntax.",
                0,
                "",
                SystemGuid.Attribute.CREX_LAVA_TEMPLATE__TEMPLATE,
                true );

            #endregion

            #region Block Type Crex Page Menu

            // BlockType: Crex Page Menu
            RockMigrationHelper.AddBlockType( "Crex Page Menu",
                "Displays a menu of child pages.",
                "~/Plugins/com_blueboxmoon/Crex/CrexPageMenu.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_PAGE_MENU );

            // Attribute for BlockType Crex Page Menu: Background Image
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image",
                "BackgroundImage",
                "",
                "The image to be displayed in the background. Suggested size is 3840x2160.",
                0,
                "",
                SystemGuid.Attribute.CREX_PAGE_MENU__BACKGROUND_IMAGE,
                false );

            // Attribute for BlockType Crex Page Menu: Background Image Url
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.TEXT,
                "Background Image Url",
                "BackgroundImageUrl",
                "",
                "The Guid or url that will be used for the background image. <span class='tip tip-lava'></span>",
                0,
                "",
                SystemGuid.Attribute.CREX_PAGE_MENU__BACKGROUND_IMAGE_URL,
                false );

            // Attribute for BlockType Crex Page Menu: Enabled Lava Commands
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.LAVA_COMMANDS,
                "Enabled Lava Commands",
                "EnabledLavaCommands",
                "",
                "The lava commands to make available when parsing the template.",
                0,
                "",
                SystemGuid.Attribute.CREX_PAGE_MENU__ENABLED_LAVA_COMMANDS,
                false );

            // Attribute for BlockType Crex Page Menu: Layout
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.TEXT,
                "Layout",
                "Layout",
                "",
                "The screen layout to use on the TV.",
                0,
                "Menu",
                SystemGuid.Attribute.CREX_PAGE_MENU__LAYOUT,
                true );

            // Attribute for BlockType Crex Page Menu: Notification Channel
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.TEXT,
                "Notification Channel",
                "NotificationChannel",
                "",
                "The content channel to use for posting notifications on the menu (only applies to Menu layout).",
                0,
                "",
                SystemGuid.Attribute.CREX_PAGE_MENU__NOTIFICATION_CHANNEL,
                false );

            // Attribute for BlockType Crex Page Menu: Notification Template
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.TEXT,
                "Notification Template",
                "NotificationTemplate",
                "",
                "The lava template to use when generating the notification items.",
                0,
                "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/Notifications.lava' %}",
                SystemGuid.Attribute.CREX_PAGE_MENU__NOTIFICATION_TEMPLATE,
                true );

            // Attribute for BlockType Crex Page Menu: Template
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_PAGE_MENU,
                Rock.SystemGuid.FieldType.TEXT,
                "Template",
                "Template",
                "",
                "The lava template to use when generating the menu items.",
                0,
                "{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PageMenuItem.lava' %}",
                SystemGuid.Attribute.CREX_PAGE_MENU__TEMPLATE,
                true );

            #endregion

            #region Block Type Crex Redirect

            // BlockType: Crex Redirect
            RockMigrationHelper.AddBlockType( "Crex Redirect",
                "Redirects the TV application to another page.",
                "~/Plugins/com_blueboxmoon/Crex/CrexRedirect.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_REDIRECT );

            // Attribute for BlockType Crex Redirect: Enabled Lava Commands
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_REDIRECT,
                Rock.SystemGuid.FieldType.LAVA_COMMANDS,
                "Enabled Lava Commands",
                "EnabledLavaCommands",
                "",
                "The lava commands to make available when parsing the templates.",
                1,
                "",
                SystemGuid.Attribute.CREX_REDIRECT__ENABLED_LAVA_COMMANDS,
                false );

            // Attribute for BlockType Crex Redirect: Entity Type
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_REDIRECT,
                Rock.SystemGuid.FieldType.ENTITYTYPE,
                "Entity Type",
                "ContextEntityType",
                "",
                "The type of entity that will provide context for this block",
                0,
                "",
                SystemGuid.Attribute.CREX_REDIRECT__ENTITY_TYPE,
                false );

            // Attribute for BlockType Crex Redirect: Url Template
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_REDIRECT,
                Rock.SystemGuid.FieldType.CODE_EDITOR,
                "Url Template",
                "UrlTemplate",
                "",
                "The URL should be in the format of '/api/crex/page/#PageId#'.",
                0,
                "",
                SystemGuid.Attribute.CREX_REDIRECT__URL_TEMPLATE,
                true );

            #endregion

            #region Block Type Crex Video

            // BlockType: Crex Video
            RockMigrationHelper.AddBlockType( "Crex Video",
                "Displays a video or life stream.",
                "~/Plugins/com_blueboxmoon/Crex/CrexVideo.ascx",
                "Blue Box Moon > Crex",
                SystemGuid.BlockType.CREX_VIDEO );

            // Attribute for BlockType Crex Video: Entity Type
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_VIDEO,
                Rock.SystemGuid.FieldType.ENTITYTYPE,
                "Entity Type",
                "ContextEntityType",
                "",
                "The type of entity that will provide context for this block",
                0,
                "",
                SystemGuid.Attribute.CREX_VIDEO__ENTITY_TYPE,
                false );

            // Attribute for BlockType Crex Video: Video Url
            RockMigrationHelper.AddBlockTypeAttribute( SystemGuid.BlockType.CREX_VIDEO,
                Rock.SystemGuid.FieldType.TEXT,
                "Video Url",
                "VideoUrl",
                "",
                "The URL of the video to be played. <span class='tip tip-lava'></span>",
                0,
                "",
                SystemGuid.Attribute.CREX_VIDEO__VIDEO_URL,
                true );

            #endregion

            #region Page Crex Applications

            // Page: Crex Applications
            RockMigrationHelper.AddPage( "5b6dbc42-8b03-4d15-8d92-aafa28fd8616",
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Crex Applications",
                @"",
                SystemGuid.Page.CREX_APPLICATIONS,
                "fa fa-tv" );

            // Block for Page Crex Applications: Page Menu
            RockMigrationHelper.AddBlock( SystemGuid.Page.CREX_APPLICATIONS,
                "",
                SystemGuid.BlockType.CORE_PAGE_MENU,
                "Page Menu",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.CREX_APPLICATIONS__PAGE_MENU );

            // Attribute Value for Block Page Menu: Template
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.CREX_APPLICATIONS__PAGE_MENU,
                "1322186a-862a-4cf1-b349-28ecb67229ba",
                @"{% include '~~/Assets/Lava/PageListAsBlocks.lava' %}" );

            #endregion

            #region Page Crex Applications > Default Application

            // Page: Default Application
            RockMigrationHelper.AddPage( SystemGuid.Page.CREX_APPLICATIONS,
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Default Application",
                @"",
                SystemGuid.Page.DEFAULT_APPLICATION,
                "fa fa-bullseye" );

            // Block for Page Default Application: Crex Page Menu
            RockMigrationHelper.AddBlock( SystemGuid.Page.DEFAULT_APPLICATION,
                "",
                SystemGuid.BlockType.CREX_PAGE_MENU,
                "Crex Page Menu",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.DEFAULT_APPLICATION__CREX_PAGE_MENU );

            // Attribute Value for Block Crex Page Menu: Template
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.DEFAULT_APPLICATION__CREX_PAGE_MENU,
                SystemGuid.Attribute.CREX_PAGE_MENU__TEMPLATE,
                @"{% include '~/Plugins/com_blueboxmoon/Crex/Assets/Lava/PageMenu.lava' %}" );

            #endregion

            #region Page Crex Applications > Default Application > Service Times

            // Page: Service Times
            RockMigrationHelper.AddPage( SystemGuid.Page.DEFAULT_APPLICATION,
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Service Times",
                @"",
                SystemGuid.Page.SERVICE_TIMES,
                "" );

            // Block for Page Service Times: Crex Image
            RockMigrationHelper.AddBlock( SystemGuid.Page.SERVICE_TIMES,
                "",
                SystemGuid.BlockType.CREX_IMAGE,
                "Crex Image",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.SERVICE_TIMES__CREX_IMAGE );

            #endregion

            #region Page Crex Applications > Default Application > Current Series

            // Page: Current Series
            RockMigrationHelper.AddPage( SystemGuid.Page.DEFAULT_APPLICATION,
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Current Series",
                @"",
                SystemGuid.Page.CURRENT_SERIES,
                "" );

            // Block for Page Current Series: Crex Redirect
            RockMigrationHelper.AddBlock( SystemGuid.Page.CURRENT_SERIES,
                "",
                SystemGuid.BlockType.CREX_REDIRECT,
                "Crex Redirect",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.CURRENT_SERIES__CREX_REDIRECT );

            // Attribute Value for Block Crex Redirect: Url Template
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.CURRENT_SERIES__CREX_REDIRECT,
                SystemGuid.Attribute.CREX_REDIRECT__URL_TEMPLATE,
                @"{%- comment -%} *** Find the currently active series *** {%- endcomment -%}
{%- sql -%}
SELECT [Id] FROM [Page] WHERE [Guid] = 'c2daf775-ea40-4cf2-9a53-1990555abf05'
{%- endsql -%}
{%- assign pageId = results[0].Id -%}
{%- sql -%}
SELECT
    TOP 1 CCI.[Id], CCI.[StartDateTime]
FROM [ContentChannelItem] AS CCI
INNER JOIN [ContentChannel] AS CC ON CC.[Id] = CCI.[ContentChannelId]
WHERE CC.[Guid] = 'E2C598F1-D299-1BAA-4873-8B679E3C1998'
ORDER BY CCI.[StartDateTime] DESC
{%- endsql -%}
{%- assign currentSeriesId = results[0].Id -%}

/api/crex/page/{{ pageId }}?contentItemId={{ currentSeriesId }}" );

            // Attribute Value for Block Crex Redirect: Enabled Lava Commands
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.CURRENT_SERIES__CREX_REDIRECT,
                SystemGuid.Attribute.CREX_REDIRECT__ENABLED_LAVA_COMMANDS,
                @"Sql" );

            #endregion

            #region Page Crex Applications > Default Application > Archives

            // Page: Archives
            RockMigrationHelper.AddPage( SystemGuid.Page.DEFAULT_APPLICATION,
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Archives",
                @"",
                SystemGuid.Page.ARCHIVES,
                "" );

            // Block for Page Archives: Crex Content Channel View
            RockMigrationHelper.AddBlock( SystemGuid.Page.ARCHIVES,
                "",
                SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW,
                "Crex Content Channel View",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.ARCHIVES__CREX_CONTENT_CHANNEL_VIEW );

            // Attribute Value for Block Crex Content Channel View: Content Channel
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.ARCHIVES__CREX_CONTENT_CHANNEL_VIEW,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__CONTENT_CHANNEL,
                @"e2c598f1-d299-1baa-4873-8b679e3c1998" );

            // Attribute Value for Block Crex Content Channel View: Order
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.ARCHIVES__CREX_CONTENT_CHANNEL_VIEW,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__ORDER,
                @"StartDateTime^1" );

            // Attribute Value for Block Crex Content Channel View: Detail Page
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.ARCHIVES__CREX_CONTENT_CHANNEL_VIEW,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_VIEW__DETAIL_PAGE,
                @"c2daf775-ea40-4cf2-9a53-1990555abf05" );

            #endregion

            #region Page Crex Applications > Default Application > Archives > Series Items

            // Page: Series Items
            RockMigrationHelper.AddPage( SystemGuid.Page.ARCHIVES,
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Series Items",
                @"",
                SystemGuid.Page.SERIES_ITEMS,
                "" );

            // Block for Page Series Items: Crex Content Channel Item Children
            RockMigrationHelper.AddBlock( SystemGuid.Page.SERIES_ITEMS,
                "",
                SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                "Crex Content Channel Item Children",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.SERIES_ITEMS__CREX_CONTENT_CHANNEL_ITEM_CHILDREN );

            // Attribute Value for Block Crex Content Channel Item Children: Layout
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SERIES_ITEMS__CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__LAYOUT,
                @"PosterList" );

            // Attribute Value for Block Crex Content Channel Item Children: Allowed Channel Types
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SERIES_ITEMS__CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__ALLOWED_CHANNEL_TYPES,
                @"dc1a1ef5-fa05-f4b2-4753-0ce971b65f7c" );

            // Attribute Value for Block Crex Content Channel Item Children: Background Image Url
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SERIES_ITEMS__CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__BACKGROUND_IMAGE_URL,
                @"{{ ParentItem | Attribute:'SeriesImage','RawValue' }}" );

            // Attribute Value for Block Crex Content Channel Item Children: Detail Page
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.SERIES_ITEMS__CREX_CONTENT_CHANNEL_ITEM_CHILDREN,
                SystemGuid.Attribute.CREX_CONTENT_CHANNEL_ITEM_CHILDREN__DETAIL_PAGE,
                @"8ce2bb03-78fa-4b99-98f4-b2bb60369720" );

            #endregion

            #region Page Crex Applications > Default Application > Archives > Series Items > Watch Video

            // Page: Watch Video
            RockMigrationHelper.AddPage( SystemGuid.Page.SERIES_ITEMS,
                "d65f783d-87a9-4cc9-8110-e83466a0eadb", // Full Width
                "Watch Video",
                @"",
                SystemGuid.Page.WATCH_VIDEO,
                "" );

            // Block for Page Watch Video: Crex Video
            RockMigrationHelper.AddBlock( SystemGuid.Page.WATCH_VIDEO,
                "",
                SystemGuid.BlockType.CREX_VIDEO,
                "Crex Video",
                "Main",
                @"",
                @"",
                0,
                SystemGuid.Block.WATCH_VIDEO__CREX_VIDEO );

            // Attribute Value for Block Crex Video: Entity Type
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.WATCH_VIDEO__CREX_VIDEO,
                SystemGuid.Attribute.CREX_VIDEO__ENTITY_TYPE,
                @"bf12ae64-21fb-433b-a8a4-e40e8c426dda" );

            // Attribute Value for Block Crex Video: Video Url
            RockMigrationHelper.AddBlockAttributeValue( SystemGuid.Block.WATCH_VIDEO__CREX_VIDEO,
                SystemGuid.Attribute.CREX_VIDEO__VIDEO_URL,
                @"{{ Context.ContentChannelItem | Attribute:'VideoLink' }}" );

            RockMigrationHelper.UpdatePageContext( SystemGuid.Page.WATCH_VIDEO,
                "Rock.Model.ContentChannelItem",
                "ContentItemId",
                "1E7F93CE-D339-4835-BC9E-4D8558D0807F" );

            #endregion
        }

        public override void Down()
        {
            RockMigrationHelper.DeleteBlock( SystemGuid.Block.WATCH_VIDEO__CREX_VIDEO );
            RockMigrationHelper.DeletePage( SystemGuid.Page.WATCH_VIDEO );

            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SERIES_ITEMS__CREX_CONTENT_CHANNEL_ITEM_CHILDREN );
            RockMigrationHelper.DeletePage( SystemGuid.Page.SERIES_ITEMS );

            RockMigrationHelper.DeleteBlock( SystemGuid.Block.ARCHIVES__CREX_CONTENT_CHANNEL_VIEW );
            RockMigrationHelper.DeletePage( SystemGuid.Page.ARCHIVES );

            RockMigrationHelper.DeleteBlock( SystemGuid.Block.CURRENT_SERIES__CREX_REDIRECT );
            RockMigrationHelper.DeletePage( SystemGuid.Page.CURRENT_SERIES );

            RockMigrationHelper.DeleteBlock( SystemGuid.Block.SERVICE_TIMES__CREX_IMAGE );
            RockMigrationHelper.DeletePage( SystemGuid.Page.SERVICE_TIMES );

            RockMigrationHelper.DeleteBlock( SystemGuid.Block.DEFAULT_APPLICATION__CREX_PAGE_MENU );
            RockMigrationHelper.DeletePage( SystemGuid.Page.DEFAULT_APPLICATION );

            RockMigrationHelper.DeleteBlock( SystemGuid.Block.CREX_APPLICATIONS__PAGE_MENU );
            RockMigrationHelper.DeletePage( SystemGuid.Page.CREX_APPLICATIONS );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_VIDEO );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_REDIRECT );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_PAGE_MENU );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_LAVA_TEMPLATE );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_IMAGE );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_VIEW );
            RockMigrationHelper.DeleteBlockType( SystemGuid.BlockType.CREX_CONTENT_CHANNEL_ITEM_CHILDREN );
        }
    }
}
