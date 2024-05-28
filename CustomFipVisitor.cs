using System.Globalization;
using System.Text;
using FipLang.Data;
using FipLang.Type;
using FipLang.Value;

namespace FipLang;

public class CustomFipVisitor : FipBaseVisitor<Wrapper>
{
    private readonly Repository _repository;

    public CustomFipVisitor(Repository repository)
    {
        _repository = repository;
    }

    public override Wrapper VisitIntegerAtomExp(FipParser.IntegerAtomExpContext context)
    {
        return new Wrapper
        {
            Type = Integrated.Integer,
            Value = new IntegerValue(int.Parse(context.INTEGER().GetText()))
        };
    }

    public override Wrapper VisitDoubleAtomExp(FipParser.DoubleAtomExpContext context)
    {
        return new Wrapper
        {
            Type = Integrated.Double,
            // Parse the double with InvariantCulture to avoid problems with the decimal separator
            Value = new DoubleValue(double.Parse(context.DOUBLE().GetText(), CultureInfo.InvariantCulture))
        };
    }

    public override Wrapper VisitStringAtomExp(FipParser.StringAtomExpContext context)
    {
        return new Wrapper
        {
            Type = Integrated.String,
            // Remove quotes from the string
            Value = new StringValue(context.STRING().GetText().Replace("\"", ""))
        };
    }

    public override Wrapper VisitBoolAtomExp(FipParser.BoolAtomExpContext context)
    {
        return new Wrapper
        {
            Type = Integrated.Bool,
            Value = new BoolValue(bool.Parse(context.BOOL().GetText()))
        };
    }

    public override Wrapper VisitIdentifierAtomExp(FipParser.IdentifierAtomExpContext context)
    {
        return new Wrapper
        {
            // identifier is always a string
            Type = Integrated.String,
            Value = new StringValue(context.IDENTIFIER().GetText())
        };
    }

    public override Wrapper VisitReferenceAtomExp(FipParser.ReferenceAtomExpContext context)
    {
        // get the identifier of the reference by removing the @ symbol
        var identifier = context.REFERENCE().GetText()[1..];

        // Get the value from the repository
        var referenceValue = _repository[identifier];

        // Convert the value to a wrapper with the correct type
        return referenceValue switch
        {
            StringValue stringValue => new Wrapper
                { Type = Integrated.String, Value = new StringValue(stringValue.Content) },
            IntegerValue integerValue => new Wrapper
                { Type = Integrated.Integer, Value = new IntegerValue(integerValue.Content) },
            DoubleValue doubleValue => new Wrapper
                { Type = Integrated.Double, Value = new DoubleValue(doubleValue.Content) },
            BoolValue boolValue => new Wrapper 
                { Type = Integrated.Bool, Value = new BoolValue(boolValue.Content) },
            _ => throw new InvalidOperationException("Unsupported type: " + referenceValue.GetType())
        };
    }

    public override Wrapper VisitParenthesisExp(FipParser.ParenthesisExpContext context)
    {
        return Visit(context.expression());
    }

    public override Wrapper VisitComparisonExp(FipParser.ComparisonExpContext context)
    {
        // Get the operands data
        var leftOpData = Visit(context.expression(0));
        var rightOpData = Visit(context.expression(1));

        // comparison operands must be the same type (except for integer and double)
        if (leftOpData.Type != rightOpData.Type 
            && !(leftOpData.Type == Integrated.Double && rightOpData.Type == Integrated.Integer) 
            && !(leftOpData.Type == Integrated.Integer && rightOpData.Type == Integrated.Double)
           )
            throw new InvalidOperationException("Invalid operand type: " + leftOpData.Type.ToLowerStr() +
                                                " " + rightOpData.Type.ToLowerStr());
        
        // convert to string for equality comparison
        if (context.EQUALS() != null)
            return leftOpData.Value.ToString().Equals(rightOpData.Value.ToString())
                ? new Wrapper { Type = Integrated.Bool, Value = new BoolValue(true) }
                : new Wrapper { Type = Integrated.Bool, Value = new BoolValue(false) };

        if (context.NOTEQUALS() != null)
        {
            return !leftOpData.Value.ToString().Equals(rightOpData.Value.ToString())
                ? new Wrapper { Type = Integrated.Bool, Value = new BoolValue(true) }
                : new Wrapper { Type = Integrated.Bool, Value = new BoolValue(false) };
        }

        // other comparisons must be integer or double (greater, greater equals, less, less equals)
        if (leftOpData.Type == Integrated.String || rightOpData.Type == Integrated.String)
            throw new InvalidOperationException("Invalid comparison type: " + leftOpData.Type.ToLowerStr() +
                                                " > " + rightOpData.Type.ToLowerStr());

        // convert to double for other comparisons
        var leftOpValue = double.Parse(leftOpData.Value.ToString(), CultureInfo.InvariantCulture);
        var rightOpValue = double.Parse(rightOpData.Value.ToString(), CultureInfo.InvariantCulture);

        if (context.GREATER() != null)
            return leftOpValue > rightOpValue
                ? new Wrapper { Type = Integrated.Bool, Value = new BoolValue(true) }
                : new Wrapper { Type = Integrated.Bool, Value = new BoolValue(false) };

        if (context.GREATEREQUALS() != null)
            return leftOpValue >= rightOpValue
                ? new Wrapper { Type = Integrated.Bool, Value = new BoolValue(true) }
                : new Wrapper { Type = Integrated.Bool, Value = new BoolValue(false) };

        if (context.LESS() != null)
            return leftOpValue < rightOpValue
                ? new Wrapper { Type = Integrated.Bool, Value = new BoolValue(true) }
                : new Wrapper { Type = Integrated.Bool, Value = new BoolValue(false) };

        if (context.LESSEQUALS() != null)
            return leftOpValue <= rightOpValue
                ? new Wrapper { Type = Integrated.Bool, Value = new BoolValue(true) }
                : new Wrapper { Type = Integrated.Bool, Value = new BoolValue(false) };

        throw new InvalidOperationException("Invalid comparison: " + context.GetText());
    }

    public override Wrapper VisitMulDivExp(FipParser.MulDivExpContext context)
    {
        // Get the operands data
        var leftOpData = Visit(context.expression(0));
        var rightOpData = Visit(context.expression(1));

        // Convert to double for calculation
        var leftOpDouble = double.Parse(leftOpData.Value.ToString(), CultureInfo.InvariantCulture);
        var rightOpDouble = double.Parse(rightOpData.Value.ToString(), CultureInfo.InvariantCulture);
        double calcResult = 0;

        // Check if the operands are valid (not string)
        if (leftOpData.Type == Integrated.String || rightOpData.Type == Integrated.String)
            throw new InvalidOperationException("Invalid operand type: " + leftOpData.Type.ToLowerStr() +
                                                " " + rightOpData.Type.ToLowerStr());

        // Calculate the result
        if (context.ASTERISK() != null) calcResult = leftOpDouble * rightOpDouble;
        if (context.SLASH() != null) calcResult = leftOpDouble / rightOpDouble;

        // Check if the result is integer or double and return the wrapper
        return calcResult % 1 == 0
            ? new Wrapper { Type = Integrated.Integer, Value = new IntegerValue((int)calcResult) }
            : new Wrapper { Type = Integrated.Double, Value = new DoubleValue(calcResult) };
    }

    public override Wrapper VisitAddSubExp(FipParser.AddSubExpContext context)
    {
        // Get the operands data
        var leftOpData = Visit(context.expression(0));
        var rightOpData = Visit(context.expression(1));

        // Convert to double for calculation
        var leftOpDouble = double.Parse(leftOpData.Value.ToString(), CultureInfo.InvariantCulture);
        var rightOpDouble = double.Parse(rightOpData.Value.ToString(), CultureInfo.InvariantCulture);
        double calcResult = 0;

        // Check if the operands are valid (not string)
        if (leftOpData.Type == Integrated.String || rightOpData.Type == Integrated.String)
            throw new InvalidOperationException("Invalid operand type: " + leftOpData.Type.ToLowerStr() +
                                                " " + rightOpData.Type.ToLowerStr());

        // Calculate the result
        if (context.PLUS() != null) calcResult = leftOpDouble + rightOpDouble;
        if (context.MINUS() != null) calcResult = leftOpDouble - rightOpDouble;

        // Check if the result is integer or double and return the wrapper
        return calcResult % 1 == 0
            ? new Wrapper { Type = Integrated.Integer, Value = new IntegerValue((int)calcResult) }
            : new Wrapper { Type = Integrated.Double, Value = new DoubleValue(calcResult) };
    }

    public override Wrapper VisitAssignment(FipParser.AssignmentContext context)
    {
        // Get the type , identifier and data of the assignment
        var assignType = context.VALUETYPE().GetText();
        var identifier = context.IDENTIFIER().GetText();
        var assignData = Visit(context.expression());

        // Check if the type of the data is the same as the type of the assignment
        if (assignData.Type != assignType.ToFipType())
            throw new InvalidOperationException("Invalid type assignment: " + assignData.Type.ToLowerStr() +
                                                " to " + assignType);

        // Add the data to the repository
        _repository[identifier] = assignData.Value;

        // Return the wrapper
        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }

    public override Wrapper VisitUpdate(FipParser.UpdateContext context)
    {
        var reference = context.expression(0).GetText()[1..];

        // get the reference to compare the type of the data
        var referenceData = Visit(context.expression(0));
        var updateData = Visit(context.expression(1));

        // Check if the type of the data is the same as the type of the reference
        if (referenceData.Type != updateData.Type)
            throw new InvalidOperationException("Invalid type assignment: " + updateData.Type.ToLowerStr() +
                                                " to " + referenceData.Type.ToLowerStr());

        // Update the data in the repository
        _repository[reference] = updateData.Value;

        // Return the wrapper
        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }


    public override Wrapper VisitPrint(FipParser.PrintContext context)
    {
        // Get a list of all the data to print
        var dataList = context.expression().Select(Visit).ToList();
        Wrapper printData;

        // Check if there is only one data to print or more
        if (dataList.Count == 1)
        {
            printData = dataList[0];
        }
        else
        {
            printData = new Wrapper { Type = Integrated.String, Value = new StringValue("") };

            // Concatenate the data to print
            foreach (var data in dataList)
                printData.Value = new StringValue(printData.Value.ToString() + data.Value.ToString());
        }

        return printData;
    }

    public override Wrapper VisitMem(FipParser.MemContext context)
    {
        // Check if there is a reference
        if (context.REFERENCE() != null)
        {
            // Get the data from the repository and return the wrapper
            var referenceValue = _repository[context.REFERENCE().GetText()[1..]];
            var length = referenceValue is StringValue ? $"[{referenceValue.Length}]" : "";
            return new Wrapper
            {
                Type = Integrated.String,
                Value = new StringValue(
                    $"{referenceValue.FromFipValue().ToLowerStr()}[{length}]: @{context.REFERENCE().GetText()[1..]} = {referenceValue}")
            };
        }

        // Else get all the data from the repository
        var stringBuilder = new StringBuilder();

        // Concatenate the data
        foreach (var record in _repository.Values)
        {
            var length = record.Value is StringValue ? $"[{record.Value.Length}]" : "";
            stringBuilder.AppendLine(
                $"{record.Value.FromFipValue().ToLowerStr()}[{length}]: @{record.Key} = {record.Value}");
        }

        // Remove last new line if exists
        if (stringBuilder.Length > 0) stringBuilder.Remove(stringBuilder.Length - 1, 1);

        // Return the wrapper
        return new Wrapper
        {
            Type = Integrated.String, Value = new StringValue(stringBuilder.ToString())
        };
    }

    public override Wrapper VisitFreemem(FipParser.FreememContext context)
    {
        // Check if there is a reference
        if (context.REFERENCE() != null)
        {
            // Free the reference data and return the wrapper
            _repository.Free(context.REFERENCE().GetText()[1..]);
            return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
        }

        // Else free all the data from the repository
        _repository.FreeAll();
        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }

    public override Wrapper VisitCommand(FipParser.CommandContext context)
    {
        // Check which command is called and return the context Wrapper
        if (context.assignment() != null) return Visit(context.assignment());
        if (context.print() != null) return Visit(context.print());
        if (context.update() != null) return Visit(context.update());
        if (context.mem() != null) return Visit(context.mem());
        if (context.freemem() != null) return Visit(context.freemem());

        throw new InvalidOperationException("Invalid command: " + context.GetText());
    }

    public override Wrapper VisitIfStatement(FipParser.IfStatementContext context)
    {
        // Get the condition data
        var conditionData = Visit(context.expression());

        // Check if the condition is true
        if (conditionData.Value is BoolValue { Content: true })
        {
            // Execute the if block
            Visit(context.thenstmt);
        }
        else
        {
            // Check if there is an else block
            if (context.elsestmt != null)
            {
                // Execute the else block
                Visit(context.elsestmt);
            }
        }

        // Return the wrapper
        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }
    
    
    public override Wrapper VisitCommandline(FipParser.CommandlineContext context)
    {
        return Visit(context.statement());
    }
}