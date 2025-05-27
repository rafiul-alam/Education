package Entities;
import java.lang.*;
import java.util.*;
import java.io.*;
import GUI.*;
public class Patient
{
	private String name;
	private String patientid;

	File file;
	FileWriter fwrite;
	Scanner sc;
	public Patient()
	{
	 	this.name="";
	}
    public Patient(String name,String patientid)
	{
		this.name=name;
		this.patientid=patientid;
	}
	
	public void setname(String name)
	{
		this.name=name;
	}
	
	public void setpatientid(String patientid)
	{
		this.patientid=patientid;
	}
	
	public String getname()
	{
		return name;
	}
	
	public String getpatientid()
	{
		return patientid;
	}
	public void addpatient()
	{
		try{
		file=new File("./patientdata.txt");
		file.createNewFile();
		fwrite=new FileWriter(file,true);
		fwrite.write(getname()+"\t");
		fwrite.write(getpatientid()+"\n");
		fwrite.flush();
		fwrite.close();
		}
		catch(IOException ioe)
		{
			ioe.printStackTrace();
		}
		
	}
	public boolean FindPatient(String name,String patientid)
	{
		boolean flag=false;
		file=new File("./patientdata.txt");
		
		try
		{
			sc=new Scanner(file);
			
			while(sc.hasNextLine())
			{
				String line=sc.nextLine();
				String[] value=line.split("\t");
				if(value[0].equals(name)&&value[1].equals(patientid))
				{
					flag=true;
				}
			}
			
			
		}
		catch(IOException ioe)
		{
			ioe.printStackTrace();
		}
		
		return flag;
	}
	
}