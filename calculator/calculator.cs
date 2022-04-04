// Calculator for C#

static double AtomicCalculation(string op, double first, double second)
{
  double result = 0.0;
  const double DECIMAL_FIXED = 1000000;
  switch (op)
  {
    case "+":
      result = (first * DECIMAL_FIXED + second * DECIMAL_FIXED) / DECIMAL_FIXED;
      break;
    // case "-":
    //   result = first - second;
    //   break;
    case "*":
      result = first * second;
      break;
    case "/":
      result = first / second;
      break;
    case "^":
      result = Math.Pow(first, second);
      break;
    default:
      break;
  }
  return result;
}
static double CalculateOperation(string expression)
{
  Stack<string> opStack = new Stack<string>();
  Stack<string> numStack = new Stack<string>();


  Dictionary<string, int> op = new Dictionary<string, int>();

  op.Add("+", 1);
  op.Add("-", 1);
  op.Add("*", 2);
  op.Add("/", 3);
  op.Add("^", 4);

  string cache = "";

  for (int i = 0; i < expression.Length; i++)
  {
    string sstr = expression[i].ToString();
    string toPush = sstr;

    if (op.ContainsKey(sstr))
    {
      // Check when the operator is the first character
      if (i == 0)
      {
        numStack.Push("0");
      }

      // Check 2 operator stick together.
      if (op.ContainsKey(cache))
      {
        throw new ArgumentException("Invalid expression format.");
      }

      // Check . follow by operator
      if (cache == ".")
      {
        numStack.Push(numStack.Pop() + "0");
      }

      while (opStack.Count != 0 && op[opStack.Peek()] > op[sstr])
      {
        double second = Double.Parse(numStack.Pop());
        double first = Double.Parse(numStack.Pop());
        double result = AtomicCalculation(opStack.Pop(), first, second);
        numStack.Push(result.ToString());
      }

      if (sstr == "-")
      {
        numStack.Push("-");
        toPush = "+";
      }

      opStack.Push(toPush);

    }
    else
    {
      if (sstr == "." && (op.ContainsKey(cache) || cache == ""))
      {
        toPush = "0" + toPush;
      }

      if ((!op.ContainsKey(cache) && i != 0) || cache == "-")
      {
        toPush = numStack.Pop() + toPush;
      }

      numStack.Push(toPush);

    }

    cache = sstr;
    // Console.WriteLine(sstr);
    // PrintValue(numStack, opStack);
  }

  if (op.ContainsKey(cache))
  {
    throw new ArgumentException("Invalid expression format.");
  }

  while (numStack.Count != 0 && opStack.Count != 0)
  {
    double second = Double.Parse(numStack.Pop());
    double first = Double.Parse(numStack.Pop());
    double result = AtomicCalculation(opStack.Pop(), first, second);
    numStack.Push(result.ToString());
  }
  string lastResult = numStack.Pop();
  if (lastResult == "-0") lastResult = "0";

  double returner = Double.Parse(lastResult);

  return returner;
}
