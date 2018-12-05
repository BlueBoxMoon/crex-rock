using Rock.Plugin;
using Rock.Security;

namespace com.blueboxmoon.Crex.Migrations
{
    [MigrationNumber( 2, "1.7.4" )]
    public class SetDefaultSecurity : ExtendedMigration
    {
        public override void Up()
        {
            RockMigrationHelper.AddSecurityAuthForPage( SystemGuid.Page.CREX_APPLICATIONS,
                0,
                Authorization.VIEW,
                true,
                string.Empty,
                ( int ) Rock.Model.SpecialRole.AllUsers,
                "58FB8D0B-356A-43AF-9310-46CBCAF90BF6" );
        }

        public override void Down()
        {
            RockMigrationHelper.DeleteSecurityAuth( "58FB8D0B-356A-43AF-9310-46CBCAF90BF6" );
        }
    }
}
