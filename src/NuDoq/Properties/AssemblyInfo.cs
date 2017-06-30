#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyProduct(ThisAssembly.Product)]
[assembly: AssemblyTitle(ThisAssembly.Title)]
[assembly: AssemblyDescription(ThisAssembly.Description)]
[assembly: AssemblyCompany(ThisAssembly.Company)]
[assembly: AssemblyCopyright("Copyright ©  2013")]

[assembly: AssemblyMetadata("authors", ThisAssembly.Authors)]
[assembly: AssemblyMetadata("owners", ThisAssembly.Owners)]
[assembly: AssemblyMetadata("id", ThisAssembly.Product)]
[assembly: AssemblyMetadata("version", ThisAssembly.Version)]

partial class ThisAssembly
{
    public const string Product = "NuDoq";
    public const string Title = "NuDoq: A .NET XML Documentation API";
    public const string Description = "Provides core APIs for reading and writing XML API documentation files.";

    public const string Company = "Daniel Cazzulino";
    public const string Owners = "kzu";
    public const string Authors = "Daniel Cazzulino, kzu";

    public const string Version = "https://github.com/cesarsouza/NuDoq";
}