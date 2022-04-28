using System;
using System.Collections;
using System.Collections.Generic;

namespace lexer {
public enum TokenType : uint {
  eof, Tok_lparen,Tok_rparen,Tok_mult,Tok_plus,Tok_comma,Tok_minus,Tok_divide,Tok_equal,Tok_power,Tok_id,Tok_num,Tok_param
}

class Buf<T> : IEnumerator<T> {
  IEnumerator<T> current;
  Stack<IEnumerator<T>> stack;

  Object IEnumerator.Current => Current;
  public T Current => Empty ? default(T) : current.Current;
  public bool Empty => current is null;

  public Buf(IEnumerable<T> it) {
    current = it.GetEnumerator();
    stack = new Stack<IEnumerator<T>>();
  }

  public bool MoveNext() {
    if (Empty) return false;
    var res = current.MoveNext();
    if (!res) {
      if (stack.Count > 0) {
        current = stack.Pop();
        return MoveNext();
      } else {
        current = null;
      }
    }
    return res;
  }

  public void Unshift(IEnumerable<T> it) {
    stack.Push(current);
    current = it.GetEnumerator();
  }

  public void Reset() {
    throw new NotSupportedException();
  }

  public void Dispose() {
    /* no-op */
  }
}

public class Lexer {
  public static IEnumerable<(TokenType type, dynamic attr)> lex(IEnumerable<char> input, bool debug = false) {
    var inputBuf = new Buf<char>(input);
    start:
    char curCh;
    int accSt = -1;
    string buf = "";
    string tmp = "";
    state_0:
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if(curCh == ' ') goto state_1; else if(curCh == '(') goto state_2; else if(curCh == ')') goto state_3; else if(curCh == '*') goto state_4; else if(curCh == '+') goto state_5; else if(curCh == ',') goto state_6; else if(curCh == '-') goto state_7; else if(curCh == '/') goto state_8; else if(curCh == ':') goto state_9; else if(curCh == '=') goto state_10; else if(curCh == '^') goto state_11; else if(curCh == '_'||(curCh >= 'a' && curCh <= 'z')) goto state_12; else if((curCh >= '0' && curCh <= '9')) goto state_13;
      goto end;
    state_1:
      buf += tmp;
      tmp = "";
      accSt = 1;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if(curCh == ' ') goto state_1;
      goto end;
    state_2:
      buf += tmp;
      tmp = "";
      accSt = 2;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_3:
      buf += tmp;
      tmp = "";
      accSt = 3;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_4:
      buf += tmp;
      tmp = "";
      accSt = 4;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_5:
      buf += tmp;
      tmp = "";
      accSt = 5;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_6:
      buf += tmp;
      tmp = "";
      accSt = 6;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_7:
      buf += tmp;
      tmp = "";
      accSt = 7;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_8:
      buf += tmp;
      tmp = "";
      accSt = 8;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_9:
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if((curCh >= 'a' && curCh <= 'z')) goto state_18;
      goto end;
    state_10:
      buf += tmp;
      tmp = "";
      accSt = 10;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_11:
      buf += tmp;
      tmp = "";
      accSt = 11;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    state_12:
      buf += tmp;
      tmp = "";
      accSt = 12;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if(curCh == '_'||(curCh >= '0' && curCh <= '9')||(curCh >= 'a' && curCh <= 'z')) goto state_12;
      goto end;
    state_13:
      buf += tmp;
      tmp = "";
      accSt = 13;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if(curCh == '.') goto state_14; else if(curCh == 'E'||curCh == 'e') goto state_15; else if((curCh >= '0' && curCh <= '9')) goto state_13;
      goto end;
    state_14:
      buf += tmp;
      tmp = "";
      accSt = 14;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if(curCh == 'E'||curCh == 'e') goto state_15; else if((curCh >= '0' && curCh <= '9')) goto state_14;
      goto end;
    state_15:
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if(curCh == '+'||curCh == '-') goto state_16;
      goto end;
    state_16:
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if((curCh >= '0' && curCh <= '9')) goto state_17;
      goto end;
    state_17:
      buf += tmp;
      tmp = "";
      accSt = 17;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      if((curCh >= '0' && curCh <= '9')) goto state_17;
      goto end;
    state_18:
      buf += tmp;
      tmp = "";
      accSt = 18;
      
      if(!inputBuf.MoveNext()) goto end;
      curCh = inputBuf.Current;
      tmp += curCh;
      
      goto end;
    end:
    if (tmp.Length > 0) {
      inputBuf.Unshift(tmp);
    }
    var text = buf;
    switch(accSt){
      case 1:
        if (debug) Console.Error.WriteLine($"Skipping state 1: \"{text}\"");
        goto start;
      case 2:
        if (debug) Console.Error.WriteLine($"Lexed token lparen: \"{text}\"");
        yield return (TokenType.Tok_lparen, null);
        goto start;
      case 3:
        if (debug) Console.Error.WriteLine($"Lexed token rparen: \"{text}\"");
        yield return (TokenType.Tok_rparen, null);
        goto start;
      case 4:
        if (debug) Console.Error.WriteLine($"Lexed token mult: \"{text}\"");
        yield return (TokenType.Tok_mult,  text );
        goto start;
      case 5:
        if (debug) Console.Error.WriteLine($"Lexed token plus: \"{text}\"");
        yield return (TokenType.Tok_plus,  text );
        goto start;
      case 6:
        if (debug) Console.Error.WriteLine($"Lexed token comma: \"{text}\"");
        yield return (TokenType.Tok_comma, null);
        goto start;
      case 7:
        if (debug) Console.Error.WriteLine($"Lexed token minus: \"{text}\"");
        yield return (TokenType.Tok_minus,  text );
        goto start;
      case 8:
        if (debug) Console.Error.WriteLine($"Lexed token divide: \"{text}\"");
        yield return (TokenType.Tok_divide,  text );
        goto start;
      case 10:
        if (debug) Console.Error.WriteLine($"Lexed token equal: \"{text}\"");
        yield return (TokenType.Tok_equal, null);
        goto start;
      case 11:
        if (debug) Console.Error.WriteLine($"Lexed token power: \"{text}\"");
        yield return (TokenType.Tok_power,  text );
        goto start;
      case 12:
        if (debug) Console.Error.WriteLine($"Lexed token id: \"{text}\"");
        yield return (TokenType.Tok_id,  text );
        goto start;
      case 13:
        if (debug) Console.Error.WriteLine($"Lexed token num: \"{text}\"");
        yield return (TokenType.Tok_num,  double.Parse(text) );
        goto start;
      case 14:
        if (debug) Console.Error.WriteLine($"Lexed token num: \"{text}\"");
        yield return (TokenType.Tok_num,  double.Parse(text) );
        goto start;
      case 17:
        if (debug) Console.Error.WriteLine($"Lexed token num: \"{text}\"");
        yield return (TokenType.Tok_num,  double.Parse(text) );
        goto start;
      case 18:
        if (debug) Console.Error.WriteLine($"Lexed token param: \"{text}\"");
        yield return (TokenType.Tok_param,  text[1] );
        goto start;
    }
    if (inputBuf.Empty) {
      if (debug) Console.Error.WriteLine($"Got EOF while lexing \"{text}\"");
      yield return (TokenType.eof, null);
      goto start;
    }
    throw new ApplicationException("Unexpected input: " + buf + tmp);
  }
}
}