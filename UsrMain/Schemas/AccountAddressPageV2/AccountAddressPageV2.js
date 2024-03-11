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
		},
		methods: {
			/**
			 * @inheritdoc TTerrasoft.BaseEntityPage#getParentMethod
			 * @overridden
			 */
			getParentMethod: function() {
				let method, superMethod = 
						(method = this.getParentMethod.caller) && 
						(method.$previous ||
							((method = method.$owner 
								? method 
								: method.caller) &&
							method.$owner.superclass[method.$name]));

				return superMethod;
			},
			
			/**
			 * @inheritdoc Terrasoft.BaseEntityPage#save
			 * @override
			 */
			save: function() {
				let parentSave = this.getParentMethod();
				let parentArguments = arguments;
				
				this.Terrasoft.chain(
					function(next) {
						this.checkAlreadyHasDeliveryType(next);
					},
					function() {
						if (this.get("IsAlreadyHasDeliveryType")) {
							this.Terrasoft.showErrorMessage(this.get("Resources.Strings.AlreadyHasDeliveryType"));
							this.set("IsAlreadyHasDeliveryType", false)
						} else {
							parentSave.apply(this, parentArguments);
						}
					},
					this
				)
			},
			
			/**
			 * Проверяет, имеется ли адрес с таким типом доствки у контрагента.
			 */
			checkAlreadyHasDeliveryType: function(callback, scope, args) {
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
					 
					this.Ext.callback(callback, scope || this, args || []);
				}, this);
			},
		},
		diff: /**SCHEMA_DIFF*/ [] /**SCHEMA_DIFF*/,
	};
});
