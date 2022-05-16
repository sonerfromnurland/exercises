void Main()
{
	new Platypus();
}

abstract class Mammal { 
	abstract public String chew(); 
	public Mammal() { 
		Console.WriteLine(chew()); 
	}
}

class Platypus : Mammal { 
	override public String chew() { 
	return "kibirliahmetsevgili!"; 
	}  
}
