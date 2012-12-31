using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BLToolkit;
using BLToolkit.Data;
using BLToolkit.Reflection;
using BLToolkit.Reflection.Extension;
using BLToolkit.Reflection.MetadataProvider;
using BLToolkit.Mapping;
using BLM = BLToolkit.Mapping;
using BLToolkit.DataAccess;

namespace DAF.Core.Data.BLToolkit
{
    public class DataAnnotationMetadataProvider : MetadataProviderBase
    {
        #region GetFieldName

        public override string GetFieldName(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            var colAttr = member.GetAttribute<ColumnAttribute>();
            if (colAttr != null)
            {
                if (!string.IsNullOrEmpty(colAttr.Name))
                {
                    isSet = true;
                    return colAttr.Name;
                }
            }

            return base.GetFieldName(typeExtension, member, out isSet);
        }

        #endregion

        #region GetFieldStorage

        public override string GetFieldStorage(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            var colAttr = member.GetAttribute<ColumnAttribute>();
            if (colAttr != null)
            {
                if (!string.IsNullOrEmpty(colAttr.Name))
                {
                    isSet = true;
                    return colAttr.Name;
                }
            }

            return base.GetFieldStorage(typeExtension, member, out isSet);
        }

        #endregion

        #region GetInheritanceDiscriminator

        public override bool GetInheritanceDiscriminator(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            return base.GetInheritanceDiscriminator(typeExtension, member, out isSet);
        }

        #endregion

        #region EnsureMapper

        public override void EnsureMapper(TypeAccessor typeAccessor, MappingSchema mappingSchema, EnsureMapperHandler handler)
        {
            base.EnsureMapper(typeAccessor, mappingSchema, handler);
        }

        #endregion

        #region GetMapIgnore

        public override bool GetMapIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            if (member.IsDefined<NotMappedAttribute>() || member.IsDefined<ScaffoldColumnAttribute>() ||
                member.IsDefined<System.ComponentModel.DataAnnotations.AssociationAttribute>())
            {
                isSet = true;
                return true;
            }

            if (member.IsDefined<ColumnAttribute>())
            {
                isSet = true;
                return false;
            }

            return base.GetMapIgnore(typeExtension, member, out isSet);
        }

        #endregion

        #region GetTrimmable

        public override bool GetTrimmable(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            if (member.Type == typeof(string))
            {
                isSet = true;
                return true;
            }

            return base.GetTrimmable(typeExtension, member, out isSet);
        }

        #endregion

        #region GetMapValues

        public override MapValue[] GetMapValues(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            return GetMapValues(typeExtension, member.Type, out isSet);
        }

        const FieldAttributes EnumField = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;

        static List<MapValue> GetEnumMapValues(Type type)
        {
            var list = null as List<MapValue>;
            var fields = type.GetFields();

            foreach (var fi in fields)
            {
                if ((fi.Attributes & EnumField) == EnumField)
                {
                    var enumAttributes = Attribute.GetCustomAttributes(fi, typeof(MapValueAttribute));

                    foreach (MapValueAttribute attr in enumAttributes)
                    {
                        if (list == null)
                            list = new List<MapValue>(fields.Length);

                        var origValue = Enum.Parse(type, fi.Name, false);

                        list.Add(new MapValue(origValue, attr.Values));
                    }
                }
            }

            return list;
        }

        public override MapValue[] GetMapValues(TypeExtension typeExtension, Type type, out bool isSet)
        {
            List<MapValue> list = null;

            if (TypeHelper.IsNullable(type))
                type = type.GetGenericArguments()[0];

            if (type.IsEnum)
                list = GetEnumMapValues(type);

            isSet = list != null;

            return isSet ? list.ToArray() : null;
        }

        #endregion

        #region GetDefaultValue

        public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            return GetDefaultValue(mappingSchema, typeExtension, member.Type, out isSet);
        }

        public override object GetDefaultValue(MappingSchema mappingSchema, TypeExtension typeExtension, Type type, out bool isSet)
        {
            return base.GetDefaultValue(mappingSchema, typeExtension, type, out isSet);
        }

        #endregion

        #region GetNullable

        public override bool GetNullable(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            var keyAttr = member.GetAttribute<KeyAttribute>();
            if (keyAttr != null)
            {
                isSet = true;
                return false;
            }

            var requiredAttr = member.GetAttribute<RequiredAttribute>();
            if(requiredAttr != null)
            {
                isSet = true;
                return false;
            }

            if (member.Type == typeof(string))
            {
                isSet = true;
                return true;
            }

            if (TypeHelper.IsNullableType(member.Type))
            {
                isSet = true;
                return true;
            }

            if (member.Type.IsEnum)
                return isSet = mappingSchema.GetNullValue(member.Type) != null;

            return base.GetNullable(mappingSchema, typeExtension, member, out isSet);
        }

        #endregion

        #region GetNullValue

        private static object CheckNullValue(object value, MemberAccessor member)
        {
            if (value is Type && (Type)value == typeof(DBNull))
            {
                value = DBNull.Value;

                if (member.Type == typeof(string))
                    value = null;
            }

            return value;
        }

        public override object GetNullValue(MappingSchema mappingSchema, TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            if (member.Type == typeof(string))
            {
                isSet = true;
                return null;
            }

            if (TypeHelper.IsNullableType(member.Type))
            {
                isSet = true;
                return null;
            }

            if (member.Type.IsEnum)
            {
                var value = CheckNullValue(mappingSchema.GetNullValue(member.Type), member);

                if (value != null)
                {
                    isSet = true;
                    return value;
                }
            }

            return base.GetNullValue(mappingSchema, typeExtension, member, out isSet);
        }

        #endregion

        #region GetDbName

        public override string GetDatabaseName(Type type, ExtensionList extensions, out bool isSet)
        {
            return base.GetDatabaseName(type, extensions, out isSet);
        }

        #endregion

        #region GetOwnerName

        public override string GetOwnerName(Type type, ExtensionList extensions, out bool isSet)
        {
            var tblAttrs = TypeHelper.GetAttributes(type, typeof(TableAttribute));
            if (tblAttrs != null && tblAttrs.Length > 0)
            {
                string name = ((TableAttribute)tblAttrs[0]).Schema;
                isSet = !string.IsNullOrEmpty(name);
                return name;
            }

            return base.GetOwnerName(type, extensions, out isSet);
        }

        #endregion

        #region GetTableName

        public override string GetTableName(Type type, ExtensionList extensions, out bool isSet)
        {
            var tblAttrs = TypeHelper.GetAttributes(type, typeof(TableAttribute));
            if (tblAttrs != null && tblAttrs.Length > 0)
            {
                string name = ((TableAttribute)tblAttrs[0]).Name;
                isSet = !string.IsNullOrEmpty(name);
                return name;
            }
            return base.GetTableName(type, extensions, out isSet);
        }

        #endregion

        #region GetPrimaryKeyOrder

        public override int GetPrimaryKeyOrder(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
        {
            var keyAttr = member.GetAttribute<KeyAttribute>();
            if (keyAttr != null)
            {
                isSet = true;
                var colAttr = member.GetAttribute<ColumnAttribute>();
                if (colAttr != null)
                    return colAttr.Order;
                return 0;
            }

            return base.GetPrimaryKeyOrder(type, typeExt, member, out isSet);
        }

        #endregion

        #region GetNonUpdatableFlag

        public override NonUpdatableAttribute GetNonUpdatableAttribute(Type type, TypeExtension typeExt, MemberAccessor member, out bool isSet)
        {
            var keyAttr = member.GetAttribute<KeyAttribute>();
            bool isIdentity = member.GetAttribute<DatabaseGeneratedAttribute>() != null;
            if (keyAttr != null)
            {
                isSet = true;
                return new NonUpdatableAttribute() { IsIdentity = isIdentity, OnInsert = isIdentity, OnUpdate = true };
            }
            if (isIdentity)
            {
                isSet = true;
                return new NonUpdatableAttribute() { IsIdentity = isIdentity, OnInsert = isIdentity, OnUpdate = true };
            }

            return base.GetNonUpdatableAttribute(type, typeExt, member, out isSet);
        }

        #endregion

        #region GetSqlIgnore

        public override bool GetSqlIgnore(TypeExtension typeExtension, MemberAccessor member, out bool isSet)
        {
            var ignoreAttr = member.GetAttribute<NotMappedAttribute>();
            if (ignoreAttr != null)
            {
                isSet = true;
                return true;
            }

            var scAttr = member.GetAttribute<ScaffoldColumnAttribute>();
            if (scAttr != null)
            {
                isSet = true;
                return true;
            }

            return base.GetSqlIgnore(typeExtension, member, out isSet);
        }

        #endregion

        #region GetRelations

        public override List<MapRelationBase> GetRelations(MappingSchema schema, ExtensionList typeExt, Type master, Type slave, out bool isSet)
        {
            return base.GetRelations(schema, typeExt, master, slave, out isSet);
        }

        #endregion

        #region GetAssociation

        static string[] GetTypeKeyNames(Type type)
        {
            var properties = type.GetProperties();
            var keys = properties.Where(p => p.IsDefined(typeof(KeyAttribute))).Select(p =>
                new
                {
                    Name = p.Name,
                    Order = p.IsDefined(typeof(ColumnAttribute)) ? p.GetCustomAttribute<ColumnAttribute>().Order : 0
                });
            return keys.OrderBy(k => k.Order).Select(k => k.Name).ToArray();
        }

        public override Association GetAssociation(TypeExtension typeExtension, MemberAccessor member)
        {
            if (member.Type.IsClass && !member.Type.FullName.StartsWith("System"))
            {
                bool canBeNull = !member.IsDefined<RequiredAttribute>();
                Type otherType = member.Type;
                if(otherType.IsGenericType)
                    otherType = otherType.GetGenericArguments()[0];
                string[] otherKeys = null;
                string[] thisKeys = null;
                if (member.IsDefined<System.ComponentModel.DataAnnotations.AssociationAttribute>())
                {
                    var asAttr = member.GetAttribute<System.ComponentModel.DataAnnotations.AssociationAttribute>();
                    thisKeys = asAttr.ThisKeyMembers.ToArray();
                    otherKeys = asAttr.OtherKeyMembers.ToArray();
                }
                else if (member.IsDefined<ForeignKeyAttribute>())
                {
                    var fkAttr = member.GetAttribute<ForeignKeyAttribute>();
                    otherKeys = GetTypeKeyNames(otherType);
                    thisKeys = fkAttr.Name.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    otherKeys = GetTypeKeyNames(otherType);
                    thisKeys = otherKeys;
                }

                if (thisKeys != null && otherKeys != null && thisKeys.Length == otherKeys.Length && thisKeys.Length > 0)
                {
                    return new Association(
                        member,
                        thisKeys,
                        otherKeys,
                        string.Format("FK_{0}_{1}", member.TypeAccessor.Type, otherType),
                        canBeNull);
                }
            }
            return base.GetAssociation(typeExtension, member);
        }

        #endregion

        #region GetInheritanceMapping

        public override InheritanceMappingAttribute[] GetInheritanceMapping(Type type, TypeExtension typeExtension)
        {
            return base.GetInheritanceMapping(type, typeExtension);
        }

        #endregion
    }
}
