//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./ShaderLab.g4 by ANTLR 4.7

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="ShaderLabParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7")]
[System.CLSCompliant(false)]
public interface IShaderLabVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.shader"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitShader([NotNull] ShaderLabParser.ShaderContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.properties"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperties([NotNull] ShaderLabParser.PropertiesContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty([NotNull] ShaderLabParser.PropertyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_int"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_int([NotNull] ShaderLabParser.Property_intContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_float"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_float([NotNull] ShaderLabParser.Property_floatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_range"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_range([NotNull] ShaderLabParser.Property_rangeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_color"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_color([NotNull] ShaderLabParser.Property_colorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_vector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_vector([NotNull] ShaderLabParser.Property_vectorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_2d"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_2d([NotNull] ShaderLabParser.Property_2dContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_cube"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_cube([NotNull] ShaderLabParser.Property_cubeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_3d"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_3d([NotNull] ShaderLabParser.Property_3dContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.category"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCategory([NotNull] ShaderLabParser.CategoryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.kernel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitKernel([NotNull] ShaderLabParser.KernelContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.hlsl_block"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHlsl_block([NotNull] ShaderLabParser.Hlsl_blockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.tags"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTags([NotNull] ShaderLabParser.TagsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.tag"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag([NotNull] ShaderLabParser.TagContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.common_state"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCommon_state([NotNull] ShaderLabParser.Common_stateContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.cull"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitCull([NotNull] ShaderLabParser.CullContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.ztest"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitZtest([NotNull] ShaderLabParser.ZtestContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.zwrite"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitZwrite([NotNull] ShaderLabParser.ZwriteContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.color_mask"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitColor_mask([NotNull] ShaderLabParser.Color_maskContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.meta"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMeta([NotNull] ShaderLabParser.MetaContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.range"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRange([NotNull] ShaderLabParser.RangeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.tag_key"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_key([NotNull] ShaderLabParser.Tag_keyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.tag_val"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTag_val([NotNull] ShaderLabParser.Tag_valContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.shader_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitShader_name([NotNull] ShaderLabParser.Shader_nameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.display_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDisplay_name([NotNull] ShaderLabParser.Display_nameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.texture_name"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTexture_name([NotNull] ShaderLabParser.Texture_nameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.property_identifier"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty_identifier([NotNull] ShaderLabParser.Property_identifierContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.val_int"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVal_int([NotNull] ShaderLabParser.Val_intContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.val_float"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVal_float([NotNull] ShaderLabParser.Val_floatContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.val_min"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVal_min([NotNull] ShaderLabParser.Val_minContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.val_max"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVal_max([NotNull] ShaderLabParser.Val_maxContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.val_vec4"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitVal_vec4([NotNull] ShaderLabParser.Val_vec4Context context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="ShaderLabParser.channel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitChannel([NotNull] ShaderLabParser.ChannelContext context);
}
