using System;
using System.Collections.Generic;
using Rock;
using Rock.Data;
using Rock.Web.Cache;

namespace com.blueboxmoon.Crex
{
    public class EntityContextCollection : DotLiquid.IIndexable, DotLiquid.ILiquidizable
    {
        /// <summary>
        /// The backing dictionary that contains the lazy values.
        /// </summary>
        private readonly Dictionary<object, Lazy<IEntity>> _backingDictionary = new Dictionary<object, Lazy<IEntity>>();

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object this[object key] => _backingDictionary[key].Value;

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey( object key )
        {
            return _backingDictionary.ContainsKey( key );
        }

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        public void AddEntity( EntityTypeCache entityType, int entityId )
        {
            var lazyEntity = new Lazy<IEntity>( () =>
            {
                try
                {
                    var dbContext = Reflection.GetDbContextForEntityType( entityType.GetEntityType() );
                    var serviceInstance = Reflection.GetServiceForEntityType( entityType.GetEntityType(), dbContext );

                    var getMethod = serviceInstance.GetType().GetMethod( "Get", new[] { typeof( int ) } );

                    return ( IEntity ) getMethod?.Invoke( serviceInstance, new object[] { entityId } );
                }
                catch
                {
                    return null;
                }
            } );

            _backingDictionary.Add( entityType.FriendlyName.Replace( " ", "" ), lazyEntity );
        }

        /// <summary>
        /// Gets the entity.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <returns>The IEntity object or null if not found.</returns>
        public IEntity GetEntity( EntityTypeCache entityType )
        {
            return ContainsKey( entityType.FriendlyName.Replace( " ", "" ) ) ? _backingDictionary[entityType.FriendlyName.Replace( " ", "" )].Value : null;
        }

        /// <summary>
        /// Gets the entity from the context.
        /// </summary>
        /// <typeparam name="T">The entity type</typeparam>
        /// <returns>The entity context.</returns>
        public T GetEntity<T>()
        {
            return ( T ) GetEntity( EntityTypeCache.Read( typeof( T ) ) );
        }

        /// <summary>
        /// Make this object compatible with Liquid syntax.
        /// </summary>
        /// <returns></returns>
        public object ToLiquid()
        {
            return this;
        }
    }
}
