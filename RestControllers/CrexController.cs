using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;

using com.blueboxmoon.Crex.Rest;
using Rock;
using Rock.Data;
using Rock.Model;
using Rock.Rest;
using Rock.Rest.Filters;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;

namespace com.blueboxmoon.Crex.RestControllers
{
    public class CrexController : ApiControllerBase
    {
        [HttpGet]
        [Authenticate]
        [System.Web.Http.Route( "api/crex/page/{id}" )]
        public object GetPage( int id )
        {
            var person = GetPerson();
            if ( !HttpContext.Current.Items.Contains( "CurrentPerson" ) )
            {
                HttpContext.Current.Items.Add( "CurrentPerson", person );
            }

            var pageCache = PageCache.Read( id );
            if ( pageCache == null || !pageCache.IsAuthorized( Authorization.VIEW, person ) )
            {
                return Unauthorized();
            }

            //SavePageViewInteraction( pageCache, person );

            var layoutPath = PageCache.FormatPath( pageCache.Layout.Site.Theme, pageCache.Layout.FileName );
            var cmsPage = ( RockPage ) BuildManager.CreateInstanceFromVirtualPath( layoutPath, typeof( RockPage ) );

            foreach ( var block in pageCache.Blocks )
            {
                if ( block.IsAuthorized( Authorization.VIEW, person ) )
                {
                    try
                    {
                        var control = ( RockBlock ) cmsPage.TemplateControl.LoadControl( block.BlockType.Path );
                        if ( control is ICrexBlock )
                        {
                            control.SetBlock( pageCache, block );
                            var crexBlock = ( ICrexBlock ) control;

                            var action = crexBlock.GetCrexAction( false );

                            if ( action != null && action.Template == "Redirect" )
                            {
                                return Redirect( new Uri( ( string ) action.Data, UriKind.RelativeOrAbsolute ) );
                            }

                            return action;
                        }
                    }
                    catch ( Exception e )
                    {
                        ExceptionLogService.LogException( e, HttpContext.Current );
                    }
                }
            }

            return new CrexAction();
        }
    }
}
