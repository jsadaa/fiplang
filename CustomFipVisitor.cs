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
        string identifier = context.REFERENCE().GetText()[1..];
        
        // Get the value from the repository
        IValue referenceValue = _repository[identifier];

        // Convert the value to a wrapper
        return referenceValue switch
        {
            StringValue stringValue => new Wrapper { Type = Integrated.String, Value = new StringValue(stringValue.Content) },
            IntegerValue integerValue => new Wrapper { Type = Integrated.Integer, Value = new IntegerValue(integerValue.Content) },
            DoubleValue doubleValue => new Wrapper { Type = Integrated.Double, Value = new DoubleValue(doubleValue.Content) },
            _ => throw new InvalidOperationException("Unsupported type: " + referenceValue.GetType())
        };
    }

    public override Wrapper VisitParenthesisExp(FipParser.ParenthesisExpContext context)
    {
        return Visit(context.expression());
    }

    public override Wrapper VisitMulDivExp(FipParser.MulDivExpContext context)
    {
        Wrapper leftOpData = Visit(context.expression(0));
        Wrapper rightOpData = Visit(context.expression(1));
        
        // Convert to double for calculation
        double leftOpDouble = double.Parse(leftOpData.Value.ToString(), CultureInfo.InvariantCulture);
        double rightOpDouble = double.Parse(rightOpData.Value.ToString(), CultureInfo.InvariantCulture);
        double calcResult = 0;

        // Check if the operands are valid (not string)
        if (leftOpData.Type == Integrated.String || rightOpData.Type == Integrated.String)
            throw new InvalidOperationException("Invalid operand type: " + leftOpData.Type.ToLowerStr() +
                                                " " + rightOpData.Type.ToLowerStr());
        
        // Calculate the result
        if (context.ASTERISK() != null) calcResult = leftOpDouble * rightOpDouble;
        if (context.SLASH() != null) calcResult = leftOpDouble / rightOpDouble;

        // Check if the result is integer or double and return the wrapper
        return calcResult % 1 == 0 ? new Wrapper { Type = Integrated.Integer, Value = new IntegerValue((int)calcResult) } : new Wrapper { Type = Integrated.Double, Value = new DoubleValue(calcResult) };
    }

    public override Wrapper VisitAddSubExp(FipParser.AddSubExpContext context)
    {
        Wrapper leftOpData = Visit(context.expression(0));
        Wrapper rightOpData = Visit(context.expression(1));
        
        // Convert to double for calculation
        double leftOpDouble = double.Parse(leftOpData.Value.ToString(), CultureInfo.InvariantCulture);
        double rightOpDouble = double.Parse(rightOpData.Value.ToString(), CultureInfo.InvariantCulture);
        double calcResult = 0;

        // Check if the operands are valid (not string)
        if (leftOpData.Type == Integrated.String || rightOpData.Type == Integrated.String)
            throw new InvalidOperationException("Invalid operand type: " + leftOpData.Type.ToLowerStr() +
                                                " " + rightOpData.Type.ToLowerStr());
        
        // Calculate the result
        if (context.PLUS() != null) calcResult = leftOpDouble + rightOpDouble;
        if (context.MINUS() != null) calcResult = leftOpDouble - rightOpDouble;

        // Check if the result is integer or double and return the wrapper
        return calcResult % 1 == 0 ? new Wrapper { Type = Integrated.Integer, Value = new IntegerValue((int)calcResult) } : new Wrapper { Type = Integrated.Double, Value = new DoubleValue(calcResult) };
    }

    public override Wrapper VisitAssignment(FipParser.AssignmentContext context)
    {
        string assignType = context.VALUETYPE().GetText();
        string identifier = context.IDENTIFIER().GetText();
        Wrapper assignData = Visit(context.expression());

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
        string reference = context.expression(0).GetText()[1..];
        
        // We get the reference to compare the type of the data
        Wrapper referenceData = Visit(context.expression(0));
        Wrapper updateData = Visit(context.expression(1));

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
        List<Wrapper> dataList = context.expression().Select(Visit).ToList();
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
            {
                printData.Value = new StringValue(printData.Value.ToString() + data.Value.ToString());
            }
        }

        return printData;
    }

    public override Wrapper VisitCommandline(FipParser.CommandlineContext context)
    {
        return Visit(context.command());
    }

    public override Wrapper VisitMem(FipParser.MemContext context)
    {
        // Check if there is a reference
        if (context.REFERENCE() != null)
        {
            // Get the data from the repository and return the wrapper
            IValue referenceValue = _repository[context.REFERENCE().GetText()[1..]];
            return new Wrapper
            {
                Type = Integrated.String,
                Value = new StringValue(
                    $"{referenceValue.FromFipValue().ToLowerStr()}[{referenceValue.Length}]: @{context.REFERENCE().GetText()[1..]} = {referenceValue}")
            };
        }

        // Else get all the data from the repository
        var stringBuilder = new StringBuilder();
        
        // Concatenate the data
        foreach (KeyValuePair<string?, IValue> record in _repository.Values)
        {
            stringBuilder.AppendLine(
                $"{record.Value.FromFipValue().ToLowerStr()}[{record.Value.Length}]: @{record.Key} = {record.Value}");
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

        // Free all the data from the repository
        _repository.FreeAll();
        return new Wrapper { Type = Integrated.Void, Value = new StringValue("") };
    }

    public override Wrapper VisitCommand(FipParser.CommandContext context)
    {
        // Check which command is called and return the wrapper
        if (context.assignment() != null) return Visit(context.assignment());
        if (context.print() != null) return Visit(context.print());
        if (context.update() != null) return Visit(context.update());
        if (context.mem() != null) return Visit(context.mem());
        if (context.freemem() != null) return Visit(context.freemem());

        throw new InvalidOperationException("Invalid command: " + context.GetText());
    }
}