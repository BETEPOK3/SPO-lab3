using lexer;
using System;
using System.Collections.Generic;

namespace parser {
public class Parser {
  private readonly bool debug;
  private Stack<(uint state, dynamic value)> stack = new Stack<(uint state, dynamic value)>();
  private static uint[,] Action = new uint[,] {
    {42,16,42,42,42,42,36,42,42,42,29,38,7},
    {43,42,42,42,42,42,42,42,42,42,42,42,42},
    {1,42,42,42,42,42,42,42,42,42,42,42,42},
    {44,42,42,42,39,42,33,42,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {42,6,42,42,42,42,42,42,11,42,42,42,42},
    {42,42,42,42,42,42,42,42,42,42,22,42,42},
    {42,42,42,42,42,42,42,42,42,42,5,42,42},
    {42,42,42,42,42,42,42,42,4,42,42,42,42},
    {42,42,8,42,42,42,42,42,42,42,42,42,42},
    {45,42,42,42,39,42,33,42,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {46,42,42,42,39,42,33,42,42,42,42,42,42},
    {47,42,47,34,47,47,47,25,42,42,42,42,42},
    {48,42,48,48,48,48,48,48,42,40,42,42,42},
    {49,42,49,49,49,49,49,49,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {50,42,50,50,50,50,50,50,42,50,42,42,42},
    {42,42,17,42,39,42,33,42,42,42,42,42,42},
    {42,42,51,42,39,20,33,42,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {42,42,52,42,42,42,42,42,42,42,42,42,42},
    {42,42,53,42,42,23,42,42,42,42,42,42,42},
    {42,42,42,42,42,42,42,42,42,42,22,42,42},
    {42,42,54,42,42,42,42,42,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {55,42,55,55,55,55,55,55,42,42,42,42,42},
    {56,42,56,34,56,56,56,25,42,42,42,42,42},
    {57,42,57,34,57,57,57,25,42,42,42,42,42},
    {58,30,58,58,58,58,58,58,42,58,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {59,42,59,59,59,59,59,59,42,59,42,42,42},
    {42,42,31,42,42,42,42,42,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {60,42,60,60,60,60,60,60,42,42,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {61,42,61,61,61,61,61,61,42,61,42,42,42},
    {62,42,62,62,62,62,62,62,42,62,42,42,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {42,16,42,42,42,42,36,42,42,42,29,38,42},
    {63,42,63,63,63,63,63,63,42,42,42,42,42}
  };
  private static uint[,] GOTO = new uint[,] {
    {3,15,2,13,14,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {10,15,0,13,14,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,9,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {12,15,0,13,14,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {18,15,0,13,14,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {19,15,0,13,14,0,21},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,24,0},
    {0,0,0,0,0,0,0},
    {0,26,0,0,14,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {19,15,0,13,14,0,32},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,15,0,27,14,0,0},
    {0,35,0,0,14,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,37,0,0},
    {0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0},
    {0,15,0,28,14,0,0},
    {0,41,0,0,14,0,0},
    {0,0,0,0,0,0,0}
  };
  private uint top() {
    return stack.Count == 0 ? 0 : stack.Peek().state;
  }
  static string[] stateNames = new string[] {".","%eof","S","E","equal","id","lparen","param","rparen","D1","E","equal","E","T","V","F","lparen","rparen","E","E","comma","D2","id","comma","D1","divide","F","T","T","id","lparen","rparen","D2","minus","mult","F","minus","V","num","plus","power","F"};
  static string[] expectedSyms = new string[] {"S","%eof","%eof","%eof/minus/plus","E","lparen/equal","D1","id/id","equal","rparen","%eof/minus/plus","E","%eof/minus/plus","%eof/comma/minus/plus/rparen/divide/mult","%eof/comma/divide/minus/mult/plus/rparen/power","%eof/comma/divide/minus/mult/plus/rparen","E","%eof/comma/divide/minus/mult/plus/power/rparen","rparen/minus/plus","rparen/comma/minus/plus","D2","rparen","rparen/comma","D1","rparen","F","%eof/comma/divide/minus/mult/plus/rparen","divide/%eof/comma/minus/plus/rparen/mult","divide/mult/%eof/comma/minus/plus/rparen","%eof/comma/divide/minus/mult/plus/power/rparen/lparen","D2","%eof/comma/divide/minus/mult/plus/power/rparen","rparen","T","F","%eof/comma/divide/minus/mult/plus/rparen","V","%eof/comma/divide/minus/mult/plus/power/rparen","%eof/comma/divide/minus/mult/plus/power/rparen","T","F","%eof/comma/divide/minus/mult/plus/rparen"};

  public Parser(bool debug = false) {
    this.debug = debug;
  }
  public dynamic parse(IEnumerable<(TokenType type, dynamic attr)> tokens) {
    stack.Clear();
    var iter = tokens.GetEnumerator();
    iter.MoveNext();
    var a = iter.Current;
    while (true) {
      var action = Action[top(), (int)a.type];
      switch (action) {
      case 43: {
          stack.Pop();
          return stack.Pop().value;
        }
      case 53: {
          if(debug) Console.Error.WriteLine("Reduce using D1 -> id");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 5 /*D1*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<string>(){ _1 })));
          break;
        }
      case 54: {
          if(debug) Console.Error.WriteLine("Reduce using D1 -> id comma D1");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 5 /*D1*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<string>(_3){ _1 })));
          break;
        }
      case 51: {
          if(debug) Console.Error.WriteLine("Reduce using D2 -> E");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 6 /*D2*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<Node>(){ _1 })));
          break;
        }
      case 52: {
          if(debug) Console.Error.WriteLine("Reduce using D2 -> E comma D2");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 6 /*D2*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new List<Node>(_3){ _1 })));
          break;
        }
      case 56: {
          if(debug) Console.Error.WriteLine("Reduce using E -> E minus T");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 0 /*E*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_minus, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 57: {
          if(debug) Console.Error.WriteLine("Reduce using E -> E plus T");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 0 /*E*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_plus, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 47: {
          if(debug) Console.Error.WriteLine("Reduce using E -> T");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 0 /*E*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 48: {
          if(debug) Console.Error.WriteLine("Reduce using F -> V");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 1 /*F*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 63: {
          if(debug) Console.Error.WriteLine("Reduce using F -> V power F");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 1 /*F*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_power, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 46: {
          if(debug) Console.Error.WriteLine("Reduce using S -> param id equal E");
          dynamic _4=stack.Pop().value;
          var _3=stack.Pop().value.Item2;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,((_1 == 'i') ? ParserHelper.AddVar(_2, _4.SolveTree()) : throw new ApplicationException($"Parameter should be ':i'"))));
          break;
        }
      case 45: {
          if(debug) Console.Error.WriteLine("Reduce using S -> param id lparen D1 rparen equal E");
          dynamic _7=stack.Pop().value;
          var _6=stack.Pop().value.Item2;
          var _5=stack.Pop().value.Item2;
          dynamic _4=stack.Pop().value;
          var _3=stack.Pop().value.Item2;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,((_1 == 'i') ? ParserHelper.AddFunc(_2, new Func(_7, _4)) : throw new ApplicationException($"Parameter should be ':i'"))));
          break;
        }
      case 44: {
          if(debug) Console.Error.WriteLine("Reduce using S -> E");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 2 /*S*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,($"Tree: {_1.GetTree()}\nValue: {_1.SolveTree()}")));
          break;
        }
      case 49: {
          if(debug) Console.Error.WriteLine("Reduce using T -> F");
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 3 /*T*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_1)));
          break;
        }
      case 55: {
          if(debug) Console.Error.WriteLine("Reduce using T -> T divide F");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 3 /*T*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_divide, _2), new Node[]{ _1, _3}))));
          break;
        }
      case 60: {
          if(debug) Console.Error.WriteLine("Reduce using T -> T mult F");
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          dynamic _1=stack.Pop().value;
          var gt = GOTO[top(), 3 /*T*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_mult, _2), new Node[]{ _1, _3 }))));
          break;
        }
      case 58: {
          if(debug) Console.Error.WriteLine("Reduce using V -> id");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_id, _1)))));
          break;
        }
      case 59: {
          if(debug) Console.Error.WriteLine("Reduce using V -> id lparen D2 rparen");
          var _4=stack.Pop().value.Item2;
          dynamic _3=stack.Pop().value;
          var _2=stack.Pop().value.Item2;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_id, _1), _3.ToArray()))));
          break;
        }
      case 50: {
          if(debug) Console.Error.WriteLine("Reduce using V -> lparen E rparen");
          var _3=stack.Pop().value.Item2;
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(_2)));
          break;
        }
      case 61: {
          if(debug) Console.Error.WriteLine("Reduce using V -> minus V");
          dynamic _2=stack.Pop().value;
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_num, -_2)))));
          break;
        }
      case 62: {
          if(debug) Console.Error.WriteLine("Reduce using V -> num");
          var _1=stack.Pop().value.Item2;
          var gt = GOTO[top(), 4 /*V*/];
          if(gt==0) throw new ApplicationException("No goto");
          if(debug) {
            Console.Error.WriteLine($"{top()} is now on top of the stack;");
            Console.Error.WriteLine($"{gt} will be placed on the stack");
          }
          stack.Push((gt,(new Node((TokenType.Tok_num, _1)))));
          break;
        }
      case 42: {
          string parsed=stateNames[top()];
          var lastSt = top();
          while(stack.Count > 0) { stack.Pop(); parsed = stateNames[top()] + " " + parsed; }
          throw new ApplicationException(
            $"Rejection state reached after parsing \"{parsed}\", when encoutered symbol \""
            + $"\"{a.type}\" in state {lastSt}. Expected \"{expectedSyms[lastSt]}\"");
        }
      default:
        if(debug) Console.Error.WriteLine($"Shift to {action}");
        stack.Push((action, a));
        iter.MoveNext();
        a=iter.Current;
        break;
      }
    }
  }
}
}
