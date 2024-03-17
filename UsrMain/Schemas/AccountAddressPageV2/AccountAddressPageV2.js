 define("AccountAddressPageV2", [], function() {
	return {
		entitySchemaName: "AccountAddress",
		attributes: {
			/**
			 * Адрес с таким типом доствки уже имеется у контрагента.
			 */
			"IsAlreadyHasDeliveryType": {
				dataValueType: Terrasoft.DataValueType.BOOLEAN,
				value: false,
			},

			/**
			 * Тип адреса Контрагента.
			 */
			"AddressType": {
				dataValueType: this.Terrasoft.DataValueType.LOOKUP,
				dependencies: [{
					columns: [ "AddressType" ],
					methodName: "checkAlreadyHasDeliveryType"
				}]
			},
		},
		methods: {
			setValidationConfig: function() {
                this.callParent(arguments);
				 
                this.addColumnValidator("AddressType", this.addressTypeValidator);
            },
			
			addressTypeValidator: function(value) {
                let invalidMessage = "";
                let fullInvalidMessage = ""
				
				if (this.get("IsAlreadyHasDeliveryType")) {
					fullInvalidMessage = this.get("Resources.Strings.AlreadyHasDeliveryType")
					invalidMessage = this.get("Resources.Strings.AlreadyHasDeliveryType")
				}
				
				return {
					fullInvalidMessage: invalidMessage,
   					invalidMessage: invalidMessage
				};
            },
			
			/**
			 * @inheritdoc Terrasoft.BasePageV2#onEntityInitialized
			 * @overridden
			 */
			onEntityInitialized: function() {
				this.callParent(arguments);
				
				this.checkAlreadyHasDeliveryType();
			},
			
			/**
			 * Проверяет, имеется ли адрес с таким типом доствки у контрагента.
			 */
			checkAlreadyHasDeliveryType: function() {
				let accountAddress = this.Ext.create("Terrasoft.EntitySchemaQuery", "AccountAddress");

				accountAddress.addColumn("Id", "Id");
				accountAddress.addColumn("Account", "Account");
				accountAddress.addColumn("AddressType", "AddressType");

				let accountFilter = accountAddress.createColumnFilterWithParameter(
					Terrasoft.ComparisonType.EQUAL,
					"Account",
					this.get("Account")
				);
				
				let addressTypeFilter = accountAddress.createColumnFilterWithParameter(
					Terrasoft.ComparisonType.EQUAL,
					"AddressType",
					this.get("AddressType")
				);

				accountAddress.filters.add("accountFilter", accountFilter);
				accountAddress.filters.add("addressTypeFilter", addressTypeFilter);

				accountAddress.getEntityCollection(function (response) {
					if (!response.success) {
						return;
					}
					
					if (response.collection.collection.length !== 0) {
						this.set("IsAlreadyHasDeliveryType", true)
					}
				}, this);
			},
		},
		diff: /**SCHEMA_DIFF*/ [] /**SCHEMA_DIFF*/,
	};
});
