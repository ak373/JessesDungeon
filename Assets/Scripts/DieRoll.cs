using System;
using System.Collections.Generic;

public class DieRoll
{
    Random dieRoll = new Random();

    public int Rolld20() {  return dieRoll.Next(1, 21); }
}
