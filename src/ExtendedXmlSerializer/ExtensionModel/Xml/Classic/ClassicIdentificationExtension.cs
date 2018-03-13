// MIT License
// 
// Copyright (c) 2016-2018 Wojciech Nag�rski
//                    Michael DeMond
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Identification;
using ExtendedXmlSerializer.ContentModel.Reflection;
using ExtendedXmlSerializer.Core;

namespace ExtendedXmlSerializer.ExtensionModel.Xml.Classic
{
	sealed class ClassicIdentificationExtension : ISerializerExtension
	{
		public ClassicIdentificationExtension() : this(new Collection<TypeInfo>()) {}

		public ClassicIdentificationExtension(ICollection<TypeInfo> types) => Types = types;

		public ICollection<TypeInfo> Types { get; }

		public IServiceRepository Get(IServiceRepository parameter)
			=> parameter.RegisterInstance<ITypeIdentity>(TypeIdentity.Default)
			            .Register<ITypeIdentityRegistrations, TypeIdentityRegistrations>()
			            .Register<ITypeIdentifications, TypeIdentifications>()
			            .Decorate<IIdentifiers, Identifiers>()
			            .Decorate<ITypeIdentities, TypeIdentities>()
			            .Register(Register);

		ITypeIdentification Register(IServiceProvider services) => services.Get<ITypeIdentifications>()
		                                                                   .Get(Types);

		void ICommand<IServices>.Execute(IServices parameter) {}
	}
}