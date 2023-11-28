using System.Text;
using FipLang.Data;
using FipLang.Type;
using FipLang.Value;

namespace FipLang;

public class CustomFipVisitor : FipBaseVisitor<Wrapper>
{
    private readonly Repository _data;
    
    public CustomFipVisitor(Repository data)
    {
        _data = data;
    }

    public override Wrapper VisitNumericAtomExp(FipParser.NumericAtomExpContext context)
    {
        return new Wrapper
            { Type = Integrated.Integer, Value = new IntegerValue(int.Parse(context.NUMBER().GetText())) };
    }

    public override Wrapper VisitStringAtomExp(FipParser.StringAtomExpContext context)
    {
        return new Wrapper
            { Type = Integrated.String, Value = new StringValue(context.STRING().GetText().Replace("\"", "")) };
    }

    public override Wrapper VisitIdentifierAtomExp(FipParser.IdentifierAtomExpContext context)
    {
        return new Wrapper { Type = Integrated.String, Value = new StringValue(context.IDENTIFIER().GetText()) };
    }

    public override Wrapper VisitReferenceAtomExp(FipParser.ReferenceAtomExpContext context)
    {
        var identifier = context.REFERENCE().GetText()[1..];
        var refValue = _data[identifier];

        return refValue switch
        {
            StringValue stringValue => new Wrapper
            {
                Type = Integrated.String, Value = new StringValue(stringValue.Content)
            },
            IntegerValue integerValue => new Wrapper
            {
                Type = Integrated.Integer, Value = new IntegerValue(integerValue.Content)
            },
            _ => throw new InvalidOperationException("Unsupported type: " + refValue.GetType())
        };
    }

    public override Wrapper VisitParenthesisExp(FipParser.ParenthesisExpContext context)
    {
        return Visit(context.expression());
    }

    public override Wrapper VisitMulDivExp(FipParser.MulDivExpContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (left.Type != Integrated.Integer || right.Type != Integrated.Integer)
            throw new InvalidOperationException("Invalid operand type: " + left.Type.ToLowerStr() +
                                                " " + right.Type.ToLowerStr());

        var leftValue = (IntegerValue)left.Value;
        var rightValue = (IntegerValue)right.Value;

        var data = new Wrapper();

        if (context.ASTERISK() != null)
        {
            data.Type = Integrated.Integer;
            data.Value = new IntegerValue(leftValue.Content * rightValue.Content);
        }

        if (context.SLASH() != null)
        {
            data.Type = Integrated.Integer;
            data.Value = new IntegerValue(leftValue.Content / rightValue.Content);
        }

        return data;
    }

    public override Wrapper VisitAddSubExp(FipParser.AddSubExpContext context)
    {
        var left = Visit(context.expression(0));
        var right = Visit(context.expression(1));

        if (left.Type != Integrated.Integer || right.Type != Integrated.Integer)
            throw new InvalidOperationException("Invalid operand type: " + left.Type.ToLowerStr() +
                                                " " + right.Type.ToLowerStr());

        var leftValue = (IntegerValue)left.Value;
        var rightValue = (IntegerValue)right.Value;
        var data = new Wrapper();

        if (context.PLUS() != null)
        {
            data.Value = new IntegerValue(leftValue.Content + rightValue.Content);
            data.Type = Integrated.Integer;
        }

        if (context.MINUS() != null)
        {
            data.Value = new IntegerValue(leftValue.Content - rightValue.Content);
            data.Type = Integrated.Integer;
        }

        return data;
    }

    public override Wrapper VisitAssignment(FipParser.AssignmentContext context)
    {
        var type = context.VALUETYPE().GetText();
        var identifier = context.IDENTIFIER().GetText();
        var data = Visit(context.expression());

        if (data.Type != type.ToType())
            throw new InvalidOperationException("Invalid type assignment: " + data.Type.ToLowerStr() +
                                                " to " + type);

        _data[identifier] = data.Value;

        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }

    public override Wrapper VisitUpdate(FipParser.UpdateContext context)
    {
        var referenceName = context.expression(0).GetText()[1..];
        var reference = Visit(context.expression(0));
        var data = Visit(context.expression(1));

        if (reference.Type != data.Type)
            throw new InvalidOperationException("Invalid type assignment: " + data.Type.ToLowerStr() +
                                                " to " + reference.Type.ToLowerStr());

        _data[referenceName] = data.Value;

        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }


    public override Wrapper VisitPrint(FipParser.PrintContext context)
    {
        var dataList = context.expression().Select(Visit).ToList();
        Wrapper data;

        if (dataList.Count == 1)
        {
            data = dataList[0];
        }
        else
        {
            data = new Wrapper { Type = Integrated.String, Value = new StringValue("") };
            foreach (var dataWrapper in dataList)
                data.Value = new StringValue(data.Value.ToString() + dataWrapper.Value.ToString());
        }

        return data;
    }

    public override Wrapper VisitCommandline(FipParser.CommandlineContext context)
    {
        return Visit(context.command());
    }

    public override Wrapper VisitMem(FipParser.MemContext context)
    {
        if (context.REFERENCE() != null)
        {
            var referenceData = _data[context.REFERENCE().GetText()[1..]];
            return new Wrapper
            {
                Type = Integrated.String,
                Value = new StringValue(
                    $"{referenceData.FromValue().ToLowerStr()}[{referenceData.Length}]: @{context.REFERENCE().GetText()[1..]} = {referenceData}")
            };
        }

        var stringBuilder = new StringBuilder();
        foreach (var record in _data.Values)
            stringBuilder.AppendLine($"{record.Value.FromValue().ToLowerStr()}[{record.Value.Length}]: @{record.Key} = {record.Value}");

        // Remove last \n
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        
        return new Wrapper
        {
            Type = Integrated.String, Value = new StringValue(stringBuilder.ToString())
        };
    }
    
    public override Wrapper VisitFreemem(FipParser.FreememContext context)
    {
        if (context.REFERENCE() != null)
        {
            _data.Free(context.REFERENCE().GetText()[1..]);
            return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
        }

        _data.FreeAll();
        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }

    public override Wrapper VisitCommand(FipParser.CommandContext context)
    {
        if (context.assignment() != null) return Visit(context.assignment());

        if (context.print() != null) return Visit(context.print());

        if (context.update() != null) return Visit(context.update());

        if (context.mem() != null) return Visit(context.mem());
        
        if (context.freemem() != null) return Visit(context.freemem());

        throw new InvalidOperationException("Invalid command: " + context.GetText());
    }
}