//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from Fip.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="FipParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IFipListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFile([NotNull] FipParser.FileContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.file"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFile([NotNull] FipParser.FileContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.commandline"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCommandline([NotNull] FipParser.CommandlineContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.commandline"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCommandline([NotNull] FipParser.CommandlineContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>ifStatement</c>
	/// labeled alternative in <see cref="FipParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] FipParser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>ifStatement</c>
	/// labeled alternative in <see cref="FipParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] FipParser.IfStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>commandStatement</c>
	/// labeled alternative in <see cref="FipParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCommandStatement([NotNull] FipParser.CommandStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>commandStatement</c>
	/// labeled alternative in <see cref="FipParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCommandStatement([NotNull] FipParser.CommandStatementContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCommand([NotNull] FipParser.CommandContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.command"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCommand([NotNull] FipParser.CommandContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.mem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMem([NotNull] FipParser.MemContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.mem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMem([NotNull] FipParser.MemContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.freemem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFreemem([NotNull] FipParser.FreememContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.freemem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFreemem([NotNull] FipParser.FreememContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.print"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPrint([NotNull] FipParser.PrintContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.print"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPrint([NotNull] FipParser.PrintContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.update"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterUpdate([NotNull] FipParser.UpdateContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.update"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitUpdate([NotNull] FipParser.UpdateContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="FipParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssignment([NotNull] FipParser.AssignmentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="FipParser.assignment"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssignment([NotNull] FipParser.AssignmentContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>stringAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStringAtomExp([NotNull] FipParser.StringAtomExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>stringAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStringAtomExp([NotNull] FipParser.StringAtomExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>mulDivExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMulDivExp([NotNull] FipParser.MulDivExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>mulDivExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMulDivExp([NotNull] FipParser.MulDivExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>comparisonExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterComparisonExp([NotNull] FipParser.ComparisonExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>comparisonExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitComparisonExp([NotNull] FipParser.ComparisonExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>boolAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBoolAtomExp([NotNull] FipParser.BoolAtomExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>boolAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBoolAtomExp([NotNull] FipParser.BoolAtomExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>doubleAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterDoubleAtomExp([NotNull] FipParser.DoubleAtomExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>doubleAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitDoubleAtomExp([NotNull] FipParser.DoubleAtomExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>integerAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIntegerAtomExp([NotNull] FipParser.IntegerAtomExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>integerAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIntegerAtomExp([NotNull] FipParser.IntegerAtomExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>parenthesisExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParenthesisExp([NotNull] FipParser.ParenthesisExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>parenthesisExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParenthesisExp([NotNull] FipParser.ParenthesisExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>identifierAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIdentifierAtomExp([NotNull] FipParser.IdentifierAtomExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>identifierAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIdentifierAtomExp([NotNull] FipParser.IdentifierAtomExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>addSubExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAddSubExp([NotNull] FipParser.AddSubExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>addSubExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAddSubExp([NotNull] FipParser.AddSubExpContext context);
	/// <summary>
	/// Enter a parse tree produced by the <c>referenceAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReferenceAtomExp([NotNull] FipParser.ReferenceAtomExpContext context);
	/// <summary>
	/// Exit a parse tree produced by the <c>referenceAtomExp</c>
	/// labeled alternative in <see cref="FipParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReferenceAtomExp([NotNull] FipParser.ReferenceAtomExpContext context);
}
