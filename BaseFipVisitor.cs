using SpreadSheet.Result;
using SpreadSheet.Type;
using SpreadSheet.Value;

namespace FipLang;

public class BaseFipVisitor : FipBaseVisitor<DataWrapper>
{
    
    private static readonly DataRepository Data = new();

    public override DataWrapper VisitNumericAtomExp(FipParser.NumericAtomExpContext context)
    {            
        return new DataWrapper { Type = Integrated.Integer, Value = new IntegerValue(int.Parse(context.NUMBER().GetText())) };
    }
    
    public override DataWrapper VisitStringAtomExp(FipParser.StringAtomExpContext context)
    {            
        return new DataWrapper { Type = Integrated.String, Value = new StringValue(context.STRING().GetText().Replace("\"", "")) };
    }

    public override DataWrapper VisitIdentifierAtomExp(FipParser.IdentifierAtomExpContext context)
    {
        string identifier = context.IDENTIFIER().GetText();
        return new DataWrapper { Type = Integrated.String, Value = new StringValue(identifier) };
    }
    
    public override DataWrapper VisitReferenceAtomExp(FipParser.ReferenceAtomExpContext context)
    {
        string identifier = context.REFERENCE().GetText()[1..];
        var refValue = Data[identifier];

        return refValue switch
        {
            StringValue stringValue => new DataWrapper
            {
                Type = Integrated.String, Value = new StringValue(stringValue.Content)
            },
            IntegerValue integerValue => new DataWrapper
            {
                Type = Integrated.Integer, Value = new IntegerValue(integerValue.Content)
            },
            _ => throw new InvalidOperationException("Unsupported type: " + refValue.GetType())
        };
    }

    public override DataWrapper VisitParenthesisExp(FipParser.ParenthesisExpContext context)
    {
        return Visit(context.expression());
    }

    public override DataWrapper VisitMulDivExp(FipParser.MulDivExpContext context)
    {
        DataWrapper left = Visit(context.expression(0));
        DataWrapper right = Visit(context.expression(1));
        
        if (left.Type != Integrated.Integer || right.Type != Integrated.Integer)
            throw new InvalidOperationException("Invalid operand type: " + IntegratedExtensions.ToString(left.Type) + " " + IntegratedExtensions.ToString(right.Type));
        
        var leftValue = (IntegerValue) left.Value;
        var rightValue = (IntegerValue) right.Value;
        
        DataWrapper data = new DataWrapper();

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

    public override DataWrapper VisitAddSubExp(FipParser.AddSubExpContext context)
    {
        DataWrapper left = Visit(context.expression(0));
        DataWrapper right = Visit(context.expression(1));
        
        if (left.Type != Integrated.Integer || right.Type != Integrated.Integer)
            throw new InvalidOperationException("Invalid operand type: " + IntegratedExtensions.ToString(left.Type) + " " + IntegratedExtensions.ToString(right.Type));
        
        var leftValue = (IntegerValue) left.Value;
        var rightValue = (IntegerValue) right.Value;
        
        DataWrapper data = new DataWrapper();
        
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
    
    public override DataWrapper VisitAssignment(FipParser.AssignmentContext context)
    {
        string type = context.VALUETYPE().GetText();
        string identifier = context.IDENTIFIER().GetText();
        
        DataWrapper data = Visit(context.expression());
        if (data.Type != type.ToType())
            throw new InvalidOperationException("Invalid type assignment: " + IntegratedExtensions.ToString(data.Type) + " to " + type);
     
        Data[identifier] = data.Value;
        return new DataWrapper { Type = Integrated.Void, Value = new StringValue("") };
    }
    
    public override DataWrapper VisitUpdate(FipParser.UpdateContext context)
    {
        string referenceName = context.expression(0).GetText()[1..];
        DataWrapper reference = Visit(context.expression(0));
        DataWrapper data = Visit(context.expression(1));
        if (reference.Type != data.Type)
            throw new InvalidOperationException("Invalid type assignment: " + IntegratedExtensions.ToString(data.Type) + " to " + IntegratedExtensions.ToString(reference.Type));
        
        Data[referenceName] = data.Value;
        
        return new DataWrapper { Type = Integrated.Void, Value = new StringValue("") };
    }
        
    
    public override DataWrapper VisitPrint(FipParser.PrintContext context)
    {
        var dataList = context.expression().Select(Visit).ToList();

        DataWrapper data;
        
        // append all dataList data value to data
        if (dataList.Count == 1)
        {
            data = dataList[0];
        }
        else
        {
            data = new DataWrapper { Type = Integrated.String, Value = new StringValue("") };
            foreach (var dataWrapper in dataList)
            {
                data.Value = new StringValue(data.Value.ToString() + dataWrapper.Value.ToString());
            }
        }
        
        return data;
    }
    
    public override DataWrapper VisitCommandline(FipParser.CommandlineContext context)
    {
        return Visit(context.command());
    }
    
    public override DataWrapper VisitCommand(FipParser.CommandContext context)
    {
        if (context.assignment() != null)
        {
            return Visit(context.assignment());
        }

        if (context.print() != null) 
        {
            return Visit(context.print());
        }
        
        if (context.update() != null)
        {
            return Visit(context.update());
        }
        
        throw new InvalidOperationException("Invalid command: " + context.GetText());
    }
}