namespace Urlinker.Config
{
    public class UrlSecretSettings
    {
        public string ConnString {get; set;}

        

        /*
            >>>>>>> The Command for setting the User Secret settings in the Secret Store"<<<<<<<<<
            1. dotnet user-secrets init ==> This initializes a guid Id in .csproj file ...

            2. dotnet user-secrets set "UrlSecretSettings:ConnString" "My Postgre Connection String"



        */
    }
}