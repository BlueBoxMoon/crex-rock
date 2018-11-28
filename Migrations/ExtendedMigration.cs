using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using Rock.Plugin;

namespace com.blueboxmoon.Crex.Migrations
{
    public abstract class ExtendedMigration : Migration
    {
        protected void Sql( string sql, Dictionary<string, object> parameters = null )
        {
            if ( SqlConnection != null || SqlTransaction != null )
            {
                using ( SqlCommand sqlCommand = new SqlCommand( sql, SqlConnection, SqlTransaction ) )
                {
                    sqlCommand.CommandType = CommandType.Text;
                    if ( parameters != null )
                    {
                        foreach ( var key in parameters.Keys )
                        {
                            sqlCommand.Parameters.AddWithValue( key, parameters[key] );
                        }
                    }
                    sqlCommand.ExecuteNonQuery();
                }
            }
            else
            {
                throw new NullReferenceException( "The Plugin Migration requires valid SqlConnection and SqlTransaction values when executing SQL" );
            }
        }

        protected object SqlScalar( string sql, Dictionary<string, object> parameters = null )
        {
            if ( SqlConnection != null || SqlTransaction != null )
            {
                using ( SqlCommand sqlCommand = new SqlCommand( sql, SqlConnection, SqlTransaction ) )
                {
                    sqlCommand.CommandType = CommandType.Text;
                    if ( parameters != null )
                    {
                        foreach ( var key in parameters.Keys )
                        {
                            sqlCommand.Parameters.AddWithValue( key, parameters[key] );
                        }
                    }
                    return sqlCommand.ExecuteScalar();
                }
            }
            else
            {
                throw new NullReferenceException( "The Plugin Migration requires valid SqlConnection and SqlTransaction values when executing SQL" );
            }
        }

        protected SqlDataReader SqlReader( string sql, Dictionary<string, object> parameters = null )
        {
            if ( SqlConnection != null || SqlTransaction != null )
            {
                using ( SqlCommand sqlCommand = new SqlCommand( sql, SqlConnection, SqlTransaction ) )
                {
                    sqlCommand.CommandType = CommandType.Text;

                    if ( parameters != null )
                    {
                        foreach ( var key in parameters.Keys )
                        {
                            sqlCommand.Parameters.AddWithValue( key, parameters[key] );
                        }
                    }

                    return sqlCommand.ExecuteReader();
                }
            }
            else
            {
                throw new NullReferenceException( "The Plugin Migration requires valid SqlConnection and SqlTransaction values when executing SQL" );
            }
        }

        protected void Sql( string sql, string key, object value )
        {
            Sql( sql, new Dictionary<string, object> { { key, value } } );
        }

        protected object SqlScalar( string sql, string key, object value )
        {
            return SqlScalar( sql, new Dictionary<string, object> { { key, value } } );
        }

        protected SqlDataReader SqlReader( string sql, string key, object value )
        {
            return SqlReader( sql, new Dictionary<string, object> { { key, value } } );
        }

        /// <summary>
        /// Gets the entity type identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        protected int GetEntityTypeId( string guid )
        {
            return ( int ) SqlScalar( $"SELECT [Id] FROM [EntityType] WHERE [Guid] = '{ guid }'" );
        }

        /// <summary>
        /// Adds a new Service Check Type for the given Service Check Component.
        /// </summary>
        /// <param name="serviceCheckComponentGuid">The service check component GUID.</param>
        /// <param name="name">The value.</param>
        /// <param name="description">The description.</param>
        /// <param name="guid">The GUID.</param>
        protected void AddServiceCheckType( string serviceCheckComponentGuid, string name, string description, string guid )
        {
            Sql( $@"
                DECLARE @ServiceCheckComponentId int
                SET @ServiceCheckComponentId = (SELECT [Id] FROM [EntityType] WHERE [Guid] = '{ serviceCheckComponentGuid }')
                INSERT INTO [_com_blueboxmoon_WatchdogMonitor_WatchdogServiceCheckType] (
                    [EntityTypeId], [IsActive], [IsSystem],
                    [Name], [Description],
                    [CheckInterval], [RecheckInterval], [RecheckCount],
                    [Guid])
                VALUES(
                    @ServiceCheckComponentId, 1, 1,
                    '{ name }','{ description.Replace( "'", "''" ) }',
                    5, 1, 3,
                    '{ guid }')
" );
        }

        /// <summary>
        /// Adds a new Service Check Type for the given Service Check Component.
        /// </summary>
        /// <param name="serviceCheckComponentGuid">The service check component GUID.</param>
        /// <param name="name">The value.</param>
        /// <param name="description">The description.</param>
        /// <param name="guid">The GUID.</param>
        protected void AddServiceCheckType_Pre17( string serviceCheckComponentGuid, string name, string description, string guid )
        {
            Sql( $@"
                DECLARE @ServiceCheckComponentId int
                SET @ServiceCheckComponentId = (SELECT [Id] FROM [EntityType] WHERE [Guid] = '{ serviceCheckComponentGuid }')
                INSERT INTO [_com_blueboxmoon_WatchdogMonitor_WatchdogServiceCheckType] (
                    [EntityTypeId], [IsActive], [IsSystem],
                    [Name], [Description],
                    [Guid])
                VALUES(
                    @ServiceCheckComponentId, 1, 1,
                    '{ name }','{ description.Replace( "'", "''" ) }',
                    '{ guid }')
" );
        }

        /// <summary>
        /// Deletes the Service Check Type.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        protected void DeleteServiceCheckType( string guid )
        {
            RockMigrationHelper.DeleteByGuid( guid, "_com_blueboxmoon_WatchdogMonitor_WatchdogServiceCheckType" );
        }

        /// <summary>
        /// Adds the or update service check type attribute value.
        /// </summary>
        /// <param name="serviceCheckTypeGuid">The service check type unique identifier.</param>
        /// <param name="attributeGuid">The attribute unique identifier.</param>
        /// <param name="value">The value.</param>
        protected void AddOrUpdateServiceCheckTypeAttributeValue( string serviceCheckTypeGuid, string attributeGuid, string value )
        {
            var addUpdateSql = $@"
                DECLARE @ServiceCheckId int
                SET @ServiceCheckId = (SELECT [Id] FROM [_com_blueboxmoon_WatchdogMonitor_WatchdogServiceCheckType] WHERE [Guid] = '{serviceCheckTypeGuid}')
                DECLARE @AttributeId int
                SET @AttributeId = (SELECT [Id] FROM [Attribute] WHERE [Guid] = '{attributeGuid}')
                IF @ServiceCheckId IS NOT NULL AND @AttributeId IS NOT NULL
                BEGIN
                    BEGIN
                        -- Delete existing attribute value first (might have been created by Rock system)
                        DELETE [AttributeValue]
                        WHERE [AttributeId] = @AttributeId
                        AND [EntityId] = @ServiceCheckId
                        INSERT INTO [AttributeValue] (
                            [IsSystem],[AttributeId],[EntityId],
                            [Value],
                            [Guid])
                        VALUES(
                            1,@AttributeId,@ServiceCheckId,
                            '{value.Replace( "'", "''" )}',
                            NEWID())
                    END
                END
";

            Sql( addUpdateSql );
        }
    }
}
