using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
class Generator{
public static Parser<object> make_parser()
{
Parser<object> parser1 = new Parser<object>(21,100);
RGrule rule = new RGrule("start");
rule = new RGrule("P");
rule.RuleAction = (pstack) => {  stmtlist seq = (stmtlist)pstack.Pop().Value; return new prog(seq); };
parser1.Rules.Add(rule);
rule = new RGrule("SL");
rule.RuleAction = (pstack) => { pstack.Pop();  stmt st = (stmt)pstack.Pop().Value; return new stmtlist(s,null); };
parser1.Rules.Add(rule);
rule = new RGrule("SL");
rule.RuleAction = (pstack) => {  stmtlist seq = (stmtlist)pstack.Pop().Value; pstack.Pop();  stmt st = (stmt)pstack.Pop().Value; return new stmtlist(s,seq); };
parser1.Rules.Add(rule);
rule = new RGrule("S");
rule.RuleAction = (pstack) => {  String v = (String)pstack.Pop().Value; pstack.Pop(); return new labelst(x); };
parser1.Rules.Add(rule);
rule = new RGrule("S");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop(); return new printst(e); };
parser1.Rules.Add(rule);
rule = new RGrule("S");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop();  String v = (String)pstack.Pop().Value; return new assignst(v,e); };
parser1.Rules.Add(rule);
rule = new RGrule("S");
rule.RuleAction = (pstack) => {  String v = (String)pstack.Pop().Value; pstack.Pop(); return new gotost(x); };
parser1.Rules.Add(rule);
rule = new RGrule("S");
rule.RuleAction = (pstack) => {  stmt st = (stmt)pstack.Pop().Value; pstack.Pop();  expr a = (expr)pstack.Pop().Value; pstack.Pop(); return new ifst(b,st); };
parser1.Rules.Add(rule);
rule = new RGrule("S");
rule.RuleAction = (pstack) => { pstack.Pop();  stmtlist seq = (stmtlist)pstack.Pop().Value; pstack.Pop();  expr a = (expr)pstack.Pop().Value; pstack.Pop(); return new whilest(b,seq); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  Integer n = (Integer)pstack.Pop().Value; return new intexp((int)n); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  String v = (String)pstack.Pop().Value; return new varexp(v); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  String s = (String)pstack.Pop().Value; return new strexp(s); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => { pstack.Pop(); return new boolexp(true); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => { pstack.Pop(); return new boolexp(false); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop();  expr a = (expr)pstack.Pop().Value; return new sumexp(a,b); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop();  expr a = (expr)pstack.Pop().Value; return new multexp(a,b); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop();  expr a = (expr)pstack.Pop().Value; return new andexp(a,b); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop();  expr a = (expr)pstack.Pop().Value; return new eqexp(a,b); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => {  expr a = (expr)pstack.Pop().Value; pstack.Pop(); return new negexp(a); };
parser1.Rules.Add(rule);
rule = new RGrule("E");
rule.RuleAction = (pstack) => { pstack.Pop();  expr a = (expr)pstack.Pop().Value; pstack.Pop(); return a; };
parser1.Rules.Add(rule);
rule = new RGrule("START");
rule.RuleAction = (pstack) => { pstack.Pop(); pstack.Pop(); return default(object);};
parser1.Rules.Add(rule);
parser1.RSM[0].Add("SL",new GotoState(1));
parser1.RSM[0].Add("S",new GotoState(2));
parser1.RSM[0].Add("label",new Shift(3));
parser1.RSM[0].Add("print",new Shift(4));
parser1.RSM[0].Add("id",new Shift(5));
parser1.RSM[0].Add("goto",new Shift(6));
parser1.RSM[0].Add("if",new Shift(7));
parser1.RSM[0].Add("while",new Shift(8));
parser1.RSM[0].Add("P",new GotoState(9));
parser1.RSM[1].Add("EOF",new Reduce(0));
parser1.RSM[2].Add(";",new Shift(10));
parser1.RSM[3].Add("id",new Shift(11));
parser1.RSM[4].Add("E",new GotoState(12));
parser1.RSM[4].Add("num",new Shift(13));
parser1.RSM[4].Add("id",new Shift(14));
parser1.RSM[4].Add("string",new Shift(15));
parser1.RSM[4].Add("true",new Shift(16));
parser1.RSM[4].Add("false",new Shift(17));
parser1.RSM[4].Add("not",new Shift(18));
parser1.RSM[4].Add("(",new Shift(19));
parser1.RSM[5].Add("assign",new Shift(20));
parser1.RSM[6].Add("id",new Shift(21));
parser1.RSM[7].Add("E",new GotoState(22));
parser1.RSM[7].Add("num",new Shift(23));
parser1.RSM[7].Add("id",new Shift(24));
parser1.RSM[7].Add("string",new Shift(25));
parser1.RSM[7].Add("true",new Shift(26));
parser1.RSM[7].Add("false",new Shift(27));
parser1.RSM[7].Add("not",new Shift(28));
parser1.RSM[7].Add("(",new Shift(29));
parser1.RSM[8].Add("E",new GotoState(30));
parser1.RSM[8].Add("num",new Shift(31));
parser1.RSM[8].Add("id",new Shift(32));
parser1.RSM[8].Add("string",new Shift(33));
parser1.RSM[8].Add("true",new Shift(34));
parser1.RSM[8].Add("false",new Shift(35));
parser1.RSM[8].Add("not",new Shift(36));
parser1.RSM[8].Add("(",new Shift(37));
parser1.RSM[9].Add("EOF",new GotoState(38));
parser1.RSM[10].Add("EOF",new Reduce(1));
parser1.RSM[10].Add("S",new GotoState(2));
parser1.RSM[10].Add("SL",new GotoState(39));
parser1.RSM[10].Add("label",new Shift(3));
parser1.RSM[10].Add("print",new Shift(4));
parser1.RSM[10].Add("id",new Shift(5));
parser1.RSM[10].Add("goto",new Shift(6));
parser1.RSM[10].Add("if",new Shift(7));
parser1.RSM[10].Add("while",new Shift(8));
parser1.RSM[11].Add(";",new Reduce(3));
parser1.RSM[12].Add(";",new Reduce(4));
parser1.RSM[12].Add("+",new Shift(40));
parser1.RSM[12].Add("*",new Shift(41));
parser1.RSM[12].Add("and",new Shift(42));
parser1.RSM[12].Add("=",new Shift(43));
parser1.RSM[13].Add("*",new Reduce(9));
parser1.RSM[13].Add(";",new Reduce(9));
parser1.RSM[13].Add("+",new Reduce(9));
parser1.RSM[13].Add("=",new Reduce(9));
parser1.RSM[13].Add("and",new Reduce(9));
parser1.RSM[14].Add("*",new Reduce(10));
parser1.RSM[14].Add(";",new Reduce(10));
parser1.RSM[14].Add("+",new Reduce(10));
parser1.RSM[14].Add("=",new Reduce(10));
parser1.RSM[14].Add("and",new Reduce(10));
parser1.RSM[15].Add("*",new Reduce(11));
parser1.RSM[15].Add(";",new Reduce(11));
parser1.RSM[15].Add("+",new Reduce(11));
parser1.RSM[15].Add("=",new Reduce(11));
parser1.RSM[15].Add("and",new Reduce(11));
parser1.RSM[16].Add("*",new Reduce(12));
parser1.RSM[16].Add(";",new Reduce(12));
parser1.RSM[16].Add("+",new Reduce(12));
parser1.RSM[16].Add("=",new Reduce(12));
parser1.RSM[16].Add("and",new Reduce(12));
parser1.RSM[17].Add("*",new Reduce(13));
parser1.RSM[17].Add(";",new Reduce(13));
parser1.RSM[17].Add("+",new Reduce(13));
parser1.RSM[17].Add("=",new Reduce(13));
parser1.RSM[17].Add("and",new Reduce(13));
parser1.RSM[18].Add("num",new Shift(13));
parser1.RSM[18].Add("id",new Shift(14));
parser1.RSM[18].Add("string",new Shift(15));
parser1.RSM[18].Add("true",new Shift(16));
parser1.RSM[18].Add("false",new Shift(17));
parser1.RSM[18].Add("E",new GotoState(44));
parser1.RSM[18].Add("not",new Shift(18));
parser1.RSM[18].Add("(",new Shift(19));
parser1.RSM[19].Add("num",new Shift(45));
parser1.RSM[19].Add("id",new Shift(46));
parser1.RSM[19].Add("string",new Shift(47));
parser1.RSM[19].Add("true",new Shift(48));
parser1.RSM[19].Add("false",new Shift(49));
parser1.RSM[19].Add("E",new GotoState(50));
parser1.RSM[19].Add("not",new Shift(51));
parser1.RSM[19].Add("(",new Shift(52));
parser1.RSM[20].Add("E",new GotoState(53));
parser1.RSM[20].Add("num",new Shift(13));
parser1.RSM[20].Add("id",new Shift(14));
parser1.RSM[20].Add("string",new Shift(15));
parser1.RSM[20].Add("true",new Shift(16));
parser1.RSM[20].Add("false",new Shift(17));
parser1.RSM[20].Add("not",new Shift(18));
parser1.RSM[20].Add("(",new Shift(19));
parser1.RSM[21].Add(";",new Reduce(6));
parser1.RSM[22].Add("then",new Shift(54));
parser1.RSM[22].Add("+",new Shift(55));
parser1.RSM[22].Add("*",new Shift(56));
parser1.RSM[22].Add("and",new Shift(57));
parser1.RSM[22].Add("=",new Shift(58));
parser1.RSM[23].Add("*",new Reduce(9));
parser1.RSM[23].Add("+",new Reduce(9));
parser1.RSM[23].Add("=",new Reduce(9));
parser1.RSM[23].Add("and",new Reduce(9));
parser1.RSM[23].Add("then",new Reduce(9));
parser1.RSM[24].Add("*",new Reduce(10));
parser1.RSM[24].Add("+",new Reduce(10));
parser1.RSM[24].Add("=",new Reduce(10));
parser1.RSM[24].Add("and",new Reduce(10));
parser1.RSM[24].Add("then",new Reduce(10));
parser1.RSM[25].Add("*",new Reduce(11));
parser1.RSM[25].Add("+",new Reduce(11));
parser1.RSM[25].Add("=",new Reduce(11));
parser1.RSM[25].Add("and",new Reduce(11));
parser1.RSM[25].Add("then",new Reduce(11));
parser1.RSM[26].Add("*",new Reduce(12));
parser1.RSM[26].Add("+",new Reduce(12));
parser1.RSM[26].Add("=",new Reduce(12));
parser1.RSM[26].Add("and",new Reduce(12));
parser1.RSM[26].Add("then",new Reduce(12));
parser1.RSM[27].Add("*",new Reduce(13));
parser1.RSM[27].Add("+",new Reduce(13));
parser1.RSM[27].Add("=",new Reduce(13));
parser1.RSM[27].Add("and",new Reduce(13));
parser1.RSM[27].Add("then",new Reduce(13));
parser1.RSM[28].Add("num",new Shift(23));
parser1.RSM[28].Add("id",new Shift(24));
parser1.RSM[28].Add("string",new Shift(25));
parser1.RSM[28].Add("true",new Shift(26));
parser1.RSM[28].Add("false",new Shift(27));
parser1.RSM[28].Add("E",new GotoState(59));
parser1.RSM[28].Add("not",new Shift(28));
parser1.RSM[28].Add("(",new Shift(29));
parser1.RSM[29].Add("num",new Shift(45));
parser1.RSM[29].Add("id",new Shift(46));
parser1.RSM[29].Add("string",new Shift(47));
parser1.RSM[29].Add("true",new Shift(48));
parser1.RSM[29].Add("false",new Shift(49));
parser1.RSM[29].Add("E",new GotoState(60));
parser1.RSM[29].Add("not",new Shift(51));
parser1.RSM[29].Add("(",new Shift(52));
parser1.RSM[30].Add("begin",new Shift(61));
parser1.RSM[30].Add("+",new Shift(62));
parser1.RSM[30].Add("*",new Shift(63));
parser1.RSM[30].Add("and",new Shift(64));
parser1.RSM[30].Add("=",new Shift(65));
parser1.RSM[31].Add("*",new Reduce(9));
parser1.RSM[31].Add("+",new Reduce(9));
parser1.RSM[31].Add("=",new Reduce(9));
parser1.RSM[31].Add("and",new Reduce(9));
parser1.RSM[31].Add("begin",new Reduce(9));
parser1.RSM[32].Add("*",new Reduce(10));
parser1.RSM[32].Add("+",new Reduce(10));
parser1.RSM[32].Add("=",new Reduce(10));
parser1.RSM[32].Add("and",new Reduce(10));
parser1.RSM[32].Add("begin",new Reduce(10));
parser1.RSM[33].Add("*",new Reduce(11));
parser1.RSM[33].Add("+",new Reduce(11));
parser1.RSM[33].Add("=",new Reduce(11));
parser1.RSM[33].Add("and",new Reduce(11));
parser1.RSM[33].Add("begin",new Reduce(11));
parser1.RSM[34].Add("*",new Reduce(12));
parser1.RSM[34].Add("+",new Reduce(12));
parser1.RSM[34].Add("=",new Reduce(12));
parser1.RSM[34].Add("and",new Reduce(12));
parser1.RSM[34].Add("begin",new Reduce(12));
parser1.RSM[35].Add("*",new Reduce(13));
parser1.RSM[35].Add("+",new Reduce(13));
parser1.RSM[35].Add("=",new Reduce(13));
parser1.RSM[35].Add("and",new Reduce(13));
parser1.RSM[35].Add("begin",new Reduce(13));
parser1.RSM[36].Add("num",new Shift(31));
parser1.RSM[36].Add("id",new Shift(32));
parser1.RSM[36].Add("string",new Shift(33));
parser1.RSM[36].Add("true",new Shift(34));
parser1.RSM[36].Add("false",new Shift(35));
parser1.RSM[36].Add("E",new GotoState(66));
parser1.RSM[36].Add("not",new Shift(36));
parser1.RSM[36].Add("(",new Shift(37));
parser1.RSM[37].Add("num",new Shift(45));
parser1.RSM[37].Add("id",new Shift(46));
parser1.RSM[37].Add("string",new Shift(47));
parser1.RSM[37].Add("true",new Shift(48));
parser1.RSM[37].Add("false",new Shift(49));
parser1.RSM[37].Add("E",new GotoState(67));
parser1.RSM[37].Add("not",new Shift(51));
parser1.RSM[37].Add("(",new Shift(52));
parser1.RSM[38].Add("EOF",new Accept());
parser1.RSM[39].Add("EOF",new Reduce(2));
parser1.RSM[40].Add("num",new Shift(13));
parser1.RSM[40].Add("id",new Shift(14));
parser1.RSM[40].Add("string",new Shift(15));
parser1.RSM[40].Add("true",new Shift(16));
parser1.RSM[40].Add("false",new Shift(17));
parser1.RSM[40].Add("E",new GotoState(68));
parser1.RSM[40].Add("not",new Shift(18));
parser1.RSM[40].Add("(",new Shift(19));
parser1.RSM[41].Add("num",new Shift(13));
parser1.RSM[41].Add("id",new Shift(14));
parser1.RSM[41].Add("string",new Shift(15));
parser1.RSM[41].Add("true",new Shift(16));
parser1.RSM[41].Add("false",new Shift(17));
parser1.RSM[41].Add("E",new GotoState(69));
parser1.RSM[41].Add("not",new Shift(18));
parser1.RSM[41].Add("(",new Shift(19));
parser1.RSM[42].Add("num",new Shift(13));
parser1.RSM[42].Add("id",new Shift(14));
parser1.RSM[42].Add("string",new Shift(15));
parser1.RSM[42].Add("true",new Shift(16));
parser1.RSM[42].Add("false",new Shift(17));
parser1.RSM[42].Add("E",new GotoState(70));
parser1.RSM[42].Add("not",new Shift(18));
parser1.RSM[42].Add("(",new Shift(19));
parser1.RSM[43].Add("num",new Shift(13));
parser1.RSM[43].Add("id",new Shift(14));
parser1.RSM[43].Add("string",new Shift(15));
parser1.RSM[43].Add("true",new Shift(16));
parser1.RSM[43].Add("false",new Shift(17));
parser1.RSM[43].Add("E",new GotoState(71));
parser1.RSM[43].Add("not",new Shift(18));
parser1.RSM[43].Add("(",new Shift(19));
parser1.RSM[44].Add("*",new Shift(41));
parser1.RSM[44].Add(";",new Reduce(18));
parser1.RSM[44].Add("+",new Shift(40));
parser1.RSM[44].Add("=",new Shift(43));
parser1.RSM[44].Add("and",new Shift(42));
parser1.RSM[45].Add(")",new Reduce(9));
parser1.RSM[45].Add("*",new Reduce(9));
parser1.RSM[45].Add("+",new Reduce(9));
parser1.RSM[45].Add("=",new Reduce(9));
parser1.RSM[45].Add("and",new Reduce(9));
parser1.RSM[46].Add(")",new Reduce(10));
parser1.RSM[46].Add("*",new Reduce(10));
parser1.RSM[46].Add("+",new Reduce(10));
parser1.RSM[46].Add("=",new Reduce(10));
parser1.RSM[46].Add("and",new Reduce(10));
parser1.RSM[47].Add(")",new Reduce(11));
parser1.RSM[47].Add("*",new Reduce(11));
parser1.RSM[47].Add("+",new Reduce(11));
parser1.RSM[47].Add("=",new Reduce(11));
parser1.RSM[47].Add("and",new Reduce(11));
parser1.RSM[48].Add(")",new Reduce(12));
parser1.RSM[48].Add("*",new Reduce(12));
parser1.RSM[48].Add("+",new Reduce(12));
parser1.RSM[48].Add("=",new Reduce(12));
parser1.RSM[48].Add("and",new Reduce(12));
parser1.RSM[49].Add(")",new Reduce(13));
parser1.RSM[49].Add("*",new Reduce(13));
parser1.RSM[49].Add("+",new Reduce(13));
parser1.RSM[49].Add("=",new Reduce(13));
parser1.RSM[49].Add("and",new Reduce(13));
parser1.RSM[50].Add("+",new Shift(72));
parser1.RSM[50].Add("*",new Shift(73));
parser1.RSM[50].Add("and",new Shift(74));
parser1.RSM[50].Add("=",new Shift(75));
parser1.RSM[50].Add(")",new Shift(76));
parser1.RSM[51].Add("num",new Shift(45));
parser1.RSM[51].Add("id",new Shift(46));
parser1.RSM[51].Add("string",new Shift(47));
parser1.RSM[51].Add("true",new Shift(48));
parser1.RSM[51].Add("false",new Shift(49));
parser1.RSM[51].Add("E",new GotoState(77));
parser1.RSM[51].Add("not",new Shift(51));
parser1.RSM[51].Add("(",new Shift(52));
parser1.RSM[52].Add("num",new Shift(45));
parser1.RSM[52].Add("id",new Shift(46));
parser1.RSM[52].Add("string",new Shift(47));
parser1.RSM[52].Add("true",new Shift(48));
parser1.RSM[52].Add("false",new Shift(49));
parser1.RSM[52].Add("E",new GotoState(78));
parser1.RSM[52].Add("not",new Shift(51));
parser1.RSM[52].Add("(",new Shift(52));
parser1.RSM[53].Add(";",new Reduce(5));
parser1.RSM[53].Add("+",new Shift(40));
parser1.RSM[53].Add("*",new Shift(41));
parser1.RSM[53].Add("and",new Shift(42));
parser1.RSM[53].Add("=",new Shift(43));
parser1.RSM[54].Add("label",new Shift(3));
parser1.RSM[54].Add("print",new Shift(4));
parser1.RSM[54].Add("id",new Shift(5));
parser1.RSM[54].Add("goto",new Shift(6));
parser1.RSM[54].Add("if",new Shift(7));
parser1.RSM[54].Add("S",new GotoState(79));
parser1.RSM[54].Add("while",new Shift(8));
parser1.RSM[55].Add("num",new Shift(23));
parser1.RSM[55].Add("id",new Shift(24));
parser1.RSM[55].Add("string",new Shift(25));
parser1.RSM[55].Add("true",new Shift(26));
parser1.RSM[55].Add("false",new Shift(27));
parser1.RSM[55].Add("E",new GotoState(80));
parser1.RSM[55].Add("not",new Shift(28));
parser1.RSM[55].Add("(",new Shift(29));
parser1.RSM[56].Add("num",new Shift(23));
parser1.RSM[56].Add("id",new Shift(24));
parser1.RSM[56].Add("string",new Shift(25));
parser1.RSM[56].Add("true",new Shift(26));
parser1.RSM[56].Add("false",new Shift(27));
parser1.RSM[56].Add("E",new GotoState(81));
parser1.RSM[56].Add("not",new Shift(28));
parser1.RSM[56].Add("(",new Shift(29));
parser1.RSM[57].Add("num",new Shift(23));
parser1.RSM[57].Add("id",new Shift(24));
parser1.RSM[57].Add("string",new Shift(25));
parser1.RSM[57].Add("true",new Shift(26));
parser1.RSM[57].Add("false",new Shift(27));
parser1.RSM[57].Add("E",new GotoState(82));
parser1.RSM[57].Add("not",new Shift(28));
parser1.RSM[57].Add("(",new Shift(29));
parser1.RSM[58].Add("num",new Shift(23));
parser1.RSM[58].Add("id",new Shift(24));
parser1.RSM[58].Add("string",new Shift(25));
parser1.RSM[58].Add("true",new Shift(26));
parser1.RSM[58].Add("false",new Shift(27));
parser1.RSM[58].Add("E",new GotoState(83));
parser1.RSM[58].Add("not",new Shift(28));
parser1.RSM[58].Add("(",new Shift(29));
parser1.RSM[59].Add("*",new Shift(56));
parser1.RSM[59].Add("+",new Shift(55));
parser1.RSM[59].Add("=",new Shift(58));
parser1.RSM[59].Add("and",new Shift(57));
parser1.RSM[59].Add("then",new Reduce(18));
parser1.RSM[60].Add("+",new Shift(72));
parser1.RSM[60].Add("*",new Shift(73));
parser1.RSM[60].Add("and",new Shift(74));
parser1.RSM[60].Add("=",new Shift(75));
parser1.RSM[60].Add(")",new Shift(84));
parser1.RSM[61].Add("S",new GotoState(85));
parser1.RSM[61].Add("label",new Shift(3));
parser1.RSM[61].Add("print",new Shift(4));
parser1.RSM[61].Add("id",new Shift(5));
parser1.RSM[61].Add("goto",new Shift(6));
parser1.RSM[61].Add("if",new Shift(7));
parser1.RSM[61].Add("while",new Shift(8));
parser1.RSM[61].Add("SL",new GotoState(86));
parser1.RSM[62].Add("num",new Shift(31));
parser1.RSM[62].Add("id",new Shift(32));
parser1.RSM[62].Add("string",new Shift(33));
parser1.RSM[62].Add("true",new Shift(34));
parser1.RSM[62].Add("false",new Shift(35));
parser1.RSM[62].Add("E",new GotoState(87));
parser1.RSM[62].Add("not",new Shift(36));
parser1.RSM[62].Add("(",new Shift(37));
parser1.RSM[63].Add("num",new Shift(31));
parser1.RSM[63].Add("id",new Shift(32));
parser1.RSM[63].Add("string",new Shift(33));
parser1.RSM[63].Add("true",new Shift(34));
parser1.RSM[63].Add("false",new Shift(35));
parser1.RSM[63].Add("E",new GotoState(88));
parser1.RSM[63].Add("not",new Shift(36));
parser1.RSM[63].Add("(",new Shift(37));
parser1.RSM[64].Add("num",new Shift(31));
parser1.RSM[64].Add("id",new Shift(32));
parser1.RSM[64].Add("string",new Shift(33));
parser1.RSM[64].Add("true",new Shift(34));
parser1.RSM[64].Add("false",new Shift(35));
parser1.RSM[64].Add("E",new GotoState(89));
parser1.RSM[64].Add("not",new Shift(36));
parser1.RSM[64].Add("(",new Shift(37));
parser1.RSM[65].Add("num",new Shift(31));
parser1.RSM[65].Add("id",new Shift(32));
parser1.RSM[65].Add("string",new Shift(33));
parser1.RSM[65].Add("true",new Shift(34));
parser1.RSM[65].Add("false",new Shift(35));
parser1.RSM[65].Add("E",new GotoState(90));
parser1.RSM[65].Add("not",new Shift(36));
parser1.RSM[65].Add("(",new Shift(37));
parser1.RSM[66].Add("*",new Shift(63));
parser1.RSM[66].Add("+",new Shift(62));
parser1.RSM[66].Add("=",new Shift(65));
parser1.RSM[66].Add("and",new Shift(64));
parser1.RSM[66].Add("begin",new Reduce(18));
parser1.RSM[67].Add("+",new Shift(72));
parser1.RSM[67].Add("*",new Shift(73));
parser1.RSM[67].Add("and",new Shift(74));
parser1.RSM[67].Add("=",new Shift(75));
parser1.RSM[67].Add(")",new Shift(91));
parser1.RSM[68].Add("*",new Shift(41));
parser1.RSM[68].Add(";",new Reduce(14));
parser1.RSM[68].Add("+",new Shift(40));
parser1.RSM[68].Add("=",new Shift(43));
parser1.RSM[68].Add("and",new Shift(42));
parser1.RSM[69].Add("*",new Shift(41));
parser1.RSM[69].Add(";",new Reduce(15));
parser1.RSM[69].Add("+",new Shift(40));
parser1.RSM[69].Add("=",new Shift(43));
parser1.RSM[69].Add("and",new Shift(42));
parser1.RSM[70].Add("*",new Shift(41));
parser1.RSM[70].Add(";",new Reduce(16));
parser1.RSM[70].Add("+",new Shift(40));
parser1.RSM[70].Add("=",new Shift(43));
parser1.RSM[70].Add("and",new Shift(42));
parser1.RSM[71].Add("*",new Shift(41));
parser1.RSM[71].Add(";",new Reduce(17));
parser1.RSM[71].Add("+",new Shift(40));
parser1.RSM[71].Add("=",new Shift(43));
parser1.RSM[71].Add("and",new Shift(42));
parser1.RSM[72].Add("num",new Shift(45));
parser1.RSM[72].Add("id",new Shift(46));
parser1.RSM[72].Add("string",new Shift(47));
parser1.RSM[72].Add("true",new Shift(48));
parser1.RSM[72].Add("false",new Shift(49));
parser1.RSM[72].Add("E",new GotoState(92));
parser1.RSM[72].Add("not",new Shift(51));
parser1.RSM[72].Add("(",new Shift(52));
parser1.RSM[73].Add("num",new Shift(45));
parser1.RSM[73].Add("id",new Shift(46));
parser1.RSM[73].Add("string",new Shift(47));
parser1.RSM[73].Add("true",new Shift(48));
parser1.RSM[73].Add("false",new Shift(49));
parser1.RSM[73].Add("E",new GotoState(93));
parser1.RSM[73].Add("not",new Shift(51));
parser1.RSM[73].Add("(",new Shift(52));
parser1.RSM[74].Add("num",new Shift(45));
parser1.RSM[74].Add("id",new Shift(46));
parser1.RSM[74].Add("string",new Shift(47));
parser1.RSM[74].Add("true",new Shift(48));
parser1.RSM[74].Add("false",new Shift(49));
parser1.RSM[74].Add("E",new GotoState(94));
parser1.RSM[74].Add("not",new Shift(51));
parser1.RSM[74].Add("(",new Shift(52));
parser1.RSM[75].Add("num",new Shift(45));
parser1.RSM[75].Add("id",new Shift(46));
parser1.RSM[75].Add("string",new Shift(47));
parser1.RSM[75].Add("true",new Shift(48));
parser1.RSM[75].Add("false",new Shift(49));
parser1.RSM[75].Add("E",new GotoState(95));
parser1.RSM[75].Add("not",new Shift(51));
parser1.RSM[75].Add("(",new Shift(52));
parser1.RSM[76].Add("*",new Reduce(19));
parser1.RSM[76].Add(";",new Reduce(19));
parser1.RSM[76].Add("+",new Reduce(19));
parser1.RSM[76].Add("=",new Reduce(19));
parser1.RSM[76].Add("and",new Reduce(19));
parser1.RSM[77].Add(")",new Reduce(18));
parser1.RSM[77].Add("*",new Shift(73));
parser1.RSM[77].Add("+",new Shift(72));
parser1.RSM[77].Add("=",new Shift(75));
parser1.RSM[77].Add("and",new Shift(74));
parser1.RSM[78].Add("+",new Shift(72));
parser1.RSM[78].Add("*",new Shift(73));
parser1.RSM[78].Add("and",new Shift(74));
parser1.RSM[78].Add("=",new Shift(75));
parser1.RSM[78].Add(")",new Shift(96));
parser1.RSM[79].Add(";",new Reduce(7));
parser1.RSM[80].Add("*",new Shift(56));
parser1.RSM[80].Add("+",new Shift(55));
parser1.RSM[80].Add("=",new Shift(58));
parser1.RSM[80].Add("and",new Shift(57));
parser1.RSM[80].Add("then",new Reduce(14));
parser1.RSM[81].Add("*",new Shift(56));
parser1.RSM[81].Add("+",new Shift(55));
parser1.RSM[81].Add("=",new Shift(58));
parser1.RSM[81].Add("and",new Shift(57));
parser1.RSM[81].Add("then",new Reduce(15));
parser1.RSM[82].Add("*",new Shift(56));
parser1.RSM[82].Add("+",new Shift(55));
parser1.RSM[82].Add("=",new Shift(58));
parser1.RSM[82].Add("and",new Shift(57));
parser1.RSM[82].Add("then",new Reduce(16));
parser1.RSM[83].Add("*",new Shift(56));
parser1.RSM[83].Add("+",new Shift(55));
parser1.RSM[83].Add("=",new Shift(58));
parser1.RSM[83].Add("and",new Shift(57));
parser1.RSM[83].Add("then",new Reduce(17));
parser1.RSM[84].Add("*",new Reduce(19));
parser1.RSM[84].Add("+",new Reduce(19));
parser1.RSM[84].Add("=",new Reduce(19));
parser1.RSM[84].Add("and",new Reduce(19));
parser1.RSM[84].Add("then",new Reduce(19));
parser1.RSM[85].Add(";",new Shift(97));
parser1.RSM[86].Add("end",new Shift(98));
parser1.RSM[87].Add("*",new Shift(63));
parser1.RSM[87].Add("+",new Shift(62));
parser1.RSM[87].Add("=",new Shift(65));
parser1.RSM[87].Add("and",new Shift(64));
parser1.RSM[87].Add("begin",new Reduce(14));
parser1.RSM[88].Add("*",new Shift(63));
parser1.RSM[88].Add("+",new Shift(62));
parser1.RSM[88].Add("=",new Shift(65));
parser1.RSM[88].Add("and",new Shift(64));
parser1.RSM[88].Add("begin",new Reduce(15));
parser1.RSM[89].Add("*",new Shift(63));
parser1.RSM[89].Add("+",new Shift(62));
parser1.RSM[89].Add("=",new Shift(65));
parser1.RSM[89].Add("and",new Shift(64));
parser1.RSM[89].Add("begin",new Reduce(16));
parser1.RSM[90].Add("*",new Shift(63));
parser1.RSM[90].Add("+",new Shift(62));
parser1.RSM[90].Add("=",new Shift(65));
parser1.RSM[90].Add("and",new Shift(64));
parser1.RSM[90].Add("begin",new Reduce(17));
parser1.RSM[91].Add("*",new Reduce(19));
parser1.RSM[91].Add("+",new Reduce(19));
parser1.RSM[91].Add("=",new Reduce(19));
parser1.RSM[91].Add("and",new Reduce(19));
parser1.RSM[91].Add("begin",new Reduce(19));
parser1.RSM[92].Add(")",new Reduce(14));
parser1.RSM[92].Add("*",new Shift(73));
parser1.RSM[92].Add("+",new Shift(72));
parser1.RSM[92].Add("=",new Shift(75));
parser1.RSM[92].Add("and",new Shift(74));
parser1.RSM[93].Add(")",new Reduce(15));
parser1.RSM[93].Add("*",new Shift(73));
parser1.RSM[93].Add("+",new Shift(72));
parser1.RSM[93].Add("=",new Shift(75));
parser1.RSM[93].Add("and",new Shift(74));
parser1.RSM[94].Add(")",new Reduce(16));
parser1.RSM[94].Add("*",new Shift(73));
parser1.RSM[94].Add("+",new Shift(72));
parser1.RSM[94].Add("=",new Shift(75));
parser1.RSM[94].Add("and",new Shift(74));
parser1.RSM[95].Add(")",new Reduce(17));
parser1.RSM[95].Add("*",new Shift(73));
parser1.RSM[95].Add("+",new Shift(72));
parser1.RSM[95].Add("=",new Shift(75));
parser1.RSM[95].Add("and",new Shift(74));
parser1.RSM[96].Add(")",new Reduce(19));
parser1.RSM[96].Add("*",new Reduce(19));
parser1.RSM[96].Add("+",new Reduce(19));
parser1.RSM[96].Add("=",new Reduce(19));
parser1.RSM[96].Add("and",new Reduce(19));
parser1.RSM[97].Add("end",new Reduce(1));
parser1.RSM[97].Add("S",new GotoState(85));
parser1.RSM[97].Add("SL",new GotoState(99));
parser1.RSM[97].Add("label",new Shift(3));
parser1.RSM[97].Add("print",new Shift(4));
parser1.RSM[97].Add("id",new Shift(5));
parser1.RSM[97].Add("goto",new Shift(6));
parser1.RSM[97].Add("if",new Shift(7));
parser1.RSM[97].Add("while",new Shift(8));
parser1.RSM[98].Add(";",new Reduce(8));
parser1.RSM[99].Add("end",new Reduce(2));
return parser1;
}//make_parser
} // Generator Class