 namespace Terrasoft.Configuration
{
	using System;
	using System.Collections.Generic;
	using Terrasoft.Configuration.Omnichannel.Messaging;
	using Terrasoft.Core;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Entities.Events;
	using global::Common.Logging;

	#region Class: ContactEventListener

	/// <summary>
	/// Слушатель событий сущности "Адрес контрагентов".
	/// </summary>
	/// <seealso cref="BaseEntityEventListener" />
	[EntityEventListener(SchemaName = "AccountAddress")]
	public class UsrAccountAddressEventListener : BaseEntityEventListener
	{

		#region Methods: Public

		/// <summary>
		/// Обработка сущности после добавлния записи.
		/// </summary>
		/// <param name="sender">Добавленная сущность.</param>
		/// <param name="e">The <see cref="T:Terrasoft.Core.Entities.EntityAfterEventArgs" /> instance containing the
		/// event data.</param>
		public override void OnSaved(object sender, EntityAfterEventArgs e) {
			base.OnSaved(sender, e);
			
			var entity = (Entity) sender;
        	var userConnection = entity.UserConnection;
			
			var id = entity.GetTypedColumnValue<Guid>("Id");
			var accountId = entity.GetTypedColumnValue<Guid>("AccountId");
			var addressType = entity.GetTypedColumnValue<Guid>("AddressTypeId");
			
			var esq = new EntitySchemaQuery(userConnection.EntitySchemaManager, "AccountAddress");
			
			esq.AddAllSchemaColumns();
			
			esq.Filters.Add(
					esq.CreateFilterWithParameters(
						FilterComparisonType.NotEqual, 
						"Id", 
						id));
			
			esq.Filters.Add(
					esq.CreateFilterWithParameters(
						FilterComparisonType.Equal, 
						"Account", 
						accountId));
			
			esq.Filters.Add(
					esq.CreateFilterWithParameters(
						FilterComparisonType.Equal, 
						"AddressType", 
						addressType));
						
			var addresses = esq.GetEntityCollection(userConnection);
			
			foreach (var address in addresses)
			{
				address.SetColumnValue("AddressTypeId", null);
				address.Save();
			}
		}

		#endregion

	}

	#endregion

}

