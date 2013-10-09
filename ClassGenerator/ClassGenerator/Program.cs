using System;
using System.Reflection;
using System.IO;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace ClassGenerator
{
    class EnumGenerator
    {
        CodeCompileUnit targetUnit;
        CodeTypeDeclaration targetClass;
        private const string outputFileName = "Enums.cs";

        static void Main(string[] args)
        {
            var namespaceName = "DataAPI.LINQToSQL";
            var targetUnit = new CodeCompileUnit();

            AddUsing(targetUnit);
            AddNamespace(namespaceName, targetUnit);
            
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            options.BlankLinesBetweenMembers = false;
            using (StreamWriter sourceWriter = new StreamWriter(outputFileName))
            {
                provider.GenerateCodeFromCompileUnit(
                    targetUnit, sourceWriter, options);
            }
        }
        private static void AddUsing(CodeCompileUnit targetUnit)
        {
            var globalNamespace = new CodeNamespace();
            globalNamespace.Imports.Add(new CodeNamespaceImport("System"));
            globalNamespace.Imports.Add(new CodeNamespaceImport("System.ComponentModel"));
            targetUnit.Namespaces.Add(globalNamespace);
        }
        private static void AddNamespace(string namespaceName, CodeCompileUnit targetUnit)
        {
            var targetNamespace = new CodeNamespace(namespaceName);

            AddHelperClass(targetNamespace);

            using (var dc = new DBSourceDataContext())
            {
                var dictionaries = dc.DictionaryHeaders;
                foreach (var dic in dictionaries)
                {
                    var dicEnum = new CodeTypeDeclaration(dic.Name);
                    dicEnum.Comments.Add(new CodeCommentStatement("<summary>", true));
                    dicEnum.Comments.Add(new CodeCommentStatement(dic.Description, true));
                    dicEnum.Comments.Add(new CodeCommentStatement("</summary>", true));
                    dicEnum.CustomAttributes.Add(new CodeAttributeDeclaration("DictionaryHeader", new CodeAttributeArgument("Number", new CodePrimitiveExpression(dic.Number))));
                    dicEnum.IsEnum = true;
                    foreach (var itm in dic.Dictionaries)
                    {
                        var enumItem = new CodeMemberField(dic.Name, itm.StringValue.Replace(" ", String.Empty)) 
                        {
                            InitExpression = new CodePrimitiveExpression(itm.ID),
                        };
                        enumItem.CustomAttributes.Add(new CodeAttributeDeclaration("Description", new CodeAttributeArgument(new CodePrimitiveExpression(String.Empty))));
                        dicEnum.Members.Add(enumItem);
                    };
                    targetNamespace.Types.Add(dicEnum);
                };
            };
            targetUnit.Namespaces.Add(targetNamespace);
        }
        private static void AddHelperClass(CodeNamespace targetNamespace)
        {
            var helperClass = new CodeTypeDeclaration("DictionaryHeaderAttribute")
            {
                IsClass = true
            };
            helperClass.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, "Service"));
            helperClass.BaseTypes.Add(new CodeTypeReference("Attribute"));
            var nameFiled = new CodeMemberField()
            {
                Name = "_number",
                Type = new CodeTypeReference(typeof(Int32).FullName),
                Attributes = MemberAttributes.Private
            };
            var numberProoperty = new CodeMemberProperty()
            {
                Name = "Number",
                Type = new CodeTypeReference(typeof(Int32).FullName),
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            numberProoperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_number")));
            numberProoperty.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "_number"), new CodePropertySetValueReferenceExpression()));

            helperClass.Members.Add(nameFiled);
            helperClass.Members.Add(numberProoperty);
            helperClass.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, String.Empty));
            targetNamespace.Types.Add(helperClass);
        }
    }
}
