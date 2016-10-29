using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Result
/// </summary>
public class Result
{
	public Result()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private Boolean succes;

    public Boolean Succes
    {
        get { return succes; }
        set { succes = value; }
    }
    private String message;

    public String Message
    {
        get { return message; }
        set { message = value; }
    }
}