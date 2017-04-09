// MIT License
//
// Copyright (c) 2016 Wojciech Nag�rski
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
using System.Linq;
using System.Reflection;
using ExtendedXmlSerializer.ContentModel.Xml;
using ExtendedXmlSerializer.Core;
using ExtendedXmlSerializer.Core.Sources;
using ExtendedXmlSerializer.Core.Specifications;
using JetBrains.Annotations;

namespace ExtendedXmlSerializer.ExtensionModel.References
{
	sealed class Encounters : IEncounters
	{
		readonly ISpecification<object> _specification;
		readonly IDictionary<object, Identifier> _store;

		public Encounters(IDictionary<object, Identifier> store) : this(new InstanceConditionalSpecification(), store) {}

		public Encounters(ISpecification<object> specification, IDictionary<object, Identifier> store)
		{
			_specification = specification;
			_store = store;
		}

		public bool IsSatisfiedBy(object parameter) => _specification.IsSatisfiedBy(parameter);

		public Identifier? Get(object parameter) => _store.GetStructure(parameter);
	}

	sealed class ReferenceEncounters : ReferenceCacheBase<IXmlWriter, IEncounters>, IReferenceEncounters
	{
		readonly IRootReferences _references;
		readonly IEntities _entities;
		readonly ObjectIdGenerator _generator;

		[UsedImplicitly]
		public ReferenceEncounters(IRootReferences references, IEntities entities)
			: this(references, entities, new ObjectIdGenerator()) {}

		public ReferenceEncounters(IRootReferences references, IEntities entities, ObjectIdGenerator generator)
		{
			_references = references;
			_entities = entities;
			_generator = generator;
		}

		protected override IEncounters Create(IXmlWriter parameter)
			=> new Encounters(_references.Get(parameter).ToDictionary(x => x, Get));

		Identifier Get(object parameter)
			=> new Identifier(_generator.For(parameter), _entities.Get(parameter.GetType().GetTypeInfo()));
	}
}