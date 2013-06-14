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

namespace ClariusLabs.NuDoc
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;
    using Xunit;
    using Xunit.Extensions;

    public class XmlNormalizerFixture
    {
        [Theory]
        [InlineData("One tree is in default namespace.  Other is in a namespace with a prefix.", 
                    @"<Root xmlns='http://www.northwind.com'>
                      <Child>1</Child>
                    </Root>",
                    @"<n:Root xmlns:n='http://www.northwind.com'>
                        <n:Child>1</n:Child>
                    </n:Root>", 
                    null, true)]
        [InlineData("Variation on namespace prefixes.", 
                    @"<Root xmlns='http://www.northwind.com'>
                      <a:Child xmlns:a='http://www.adventureworks.com'>1</a:Child>
                    </Root>",
                     @"<Root xmlns='http://www.northwind.com'>
                        <Child xmlns='http://www.adventureworks.com'>1</Child>
                    </Root>",
                    null, true)]
        [InlineData("Attributes are not ordered.", 
                    @"<Root a='1' b='2'>
                      <Child>1</Child>
                    </Root>",
                    @"<Root b='2' a='1'>
                        <Child>1</Child>
                    </Root>", 
                    null, true)]
        [InlineData("Attributes are not ordered, take 2.", 
                    @"<Root a='1' b='2'>
                      <Child a='a' b='b' c='c' d='d'>1</Child>
                    </Root>",
                    @"<Root b='2' a='1'>
                        <Child d='d' c='c' b='b' a='a'>1</Child>
                    </Root>",
                    null, true)]
        [InlineData("One tree has a comment.  Other does not.", 
                    "<Root><!--Comment--></Root>",
                    "<Root></Root>",
                    null, true)]
        [InlineData("One element is self-closed, the other is empty.",
                    @"<doc>
                        <summary>
                          <b>unknown tag</b>
                        </summary>
                        <remarks>
                          <code />
                        </remarks>
                    </doc>",
                    @"<doc>
                        <summary>
                            <b>unknown tag</b>
                        </summary>
                        <remarks>
                            <code>
                            </code>
                        </remarks>
                    </doc>",
                    null, true)]
        [InlineData("One tree has comment and PI., other does not.", 
                    @"<Root>
                        <!--Comment-->
                        <?xml-stylesheet href='mystyle.css' type='text/css'?>
                        <Child></Child>
                      </Root>", 
                     "<Root><Child></Child></Root>", null, true)]
        [InlineData("Element is data type of xsd:double, values are equal when normalized.",
                    "<Root>25</Root>", "<Root>+25</Root>", 
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                      <xsd:element name='Root' type='xsd:double'/>
                    </xsd:schema>", true)]
        [InlineData("Element is data type of xsd:double, values are equal when normalized.",
                    @"<Root>
                      <Child>+25e+01</Child>
                      <Child>+50.0000</Child>
                    </Root>",
                    @"<Root>
                      <Child>250</Child>
                      <Child>5e1</Child>
                    </Root>",
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                      <xsd:element name='Root'>
                        <xsd:complexType mixed='true'>
                         <xsd:choice>
                          <xsd:element
                              name='Child'
                              minOccurs='0'
                              maxOccurs='unbounded'
                              type='xsd:double'/>
                         </xsd:choice>
                        </xsd:complexType>
                      </xsd:element>
                    </xsd:schema>", true)]
        [InlineData("Variations in value representations.",
                    @"<Root>
                      <ABooleanElement>1</ABooleanElement>
                      <ADateTimeElement>2009-01-21T18:50:59.0000000-08:00</ADateTimeElement>
                      <ADecimalElement>1.0</ADecimalElement>
                      <ADoubleElement>1.0</ADoubleElement>
                      <AFloatElement>1.0</AFloatElement>
                    </Root>",
                    @"<Root>
                        <ABooleanElement>true</ABooleanElement>
                        <ADateTimeElement>2009-01-21T18:50:59-08:00</ADateTimeElement>
                        <ADecimalElement>1.0</ADecimalElement>
                        <ADoubleElement>1</ADoubleElement>
                        <AFloatElement>1</AFloatElement>
                    </Root>",
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                        <xsd:element name='Root'>
                        <xsd:complexType>
                        <xsd:all>
                        <xsd:element name='ABooleanElement' minOccurs='1' maxOccurs='1'>
                            <xsd:complexType>
                            <xsd:simpleContent>
                            <xsd:extension base='xsd:boolean'>
                            <xsd:attribute name='ADefaultBooleanAttribute' default='false'/>
                            </xsd:extension>
                            </xsd:simpleContent>
                            </xsd:complexType>
                        </xsd:element>
                        <xsd:element name='ADateTimeElement' minOccurs='1' maxOccurs='1'
                            type='xsd:dateTime'/>
                        <xsd:element name='ADecimalElement' minOccurs='1' maxOccurs='1'
                            type='xsd:decimal'/>
                        <xsd:element name='ADoubleElement' minOccurs='1' maxOccurs='1'
                            type='xsd:double'/>
                        <xsd:element name='AFloatElement' minOccurs='1' maxOccurs='1'
                            type='xsd:float'/>
                        </xsd:all>
                        </xsd:complexType>
                        </xsd:element>
                    </xsd:schema>", true)]
        [InlineData("Variations in value representations.",
                    @"<Root>
                      <Child>
                        <A>1</A>
                        <B>1.0</B>
                        <C>1.0</C>
                        <D>2009-01-21T18:50:59-08:00</D>
                      </Child>
                      <Child>
                        <A>1</A>
                        <B>1.0</B>
                        <C>1.0</C>
                        <D>2009-01-21T18:50:59-08:00</D>
                      </Child>
                      <Child>
                        <A>1</A>
                        <B>1.0</B>
                        <C>1.0</C>
                        <D>2009-01-21T18:50:59-08:00</D>
                      </Child>
                    </Root>",
                    @"<Root>
                        <Child>
                        <A>1</A>
                        <B>1.0</B>
                        <C>1.0</C>
                        <D>2009-01-21T18:50:59.0000000-08:00</D>
                        </Child>
                        <Child>
                        <A>1</A>
                        <B>1</B>
                        <C>1</C>
                        <D>2009-01-21T18:50:59.0000000-08:00</D>
                        </Child>
                        <Child>
                        <A>1</A>
                        <B>1.0</B>
                        <C>1.0</C>
                        <D>2009-01-21T18:50:59.0000000-08:00</D>
                        </Child>
                    </Root>",
                    @"<xs:schema attributeFormDefault='unqualified' elementFormDefault='qualified'
                        xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                        <xs:element name='Root'>
                        <xs:complexType>
                            <xs:sequence>
                            <xs:element maxOccurs='unbounded' name='Child'>
                                <xs:complexType>
                                <xs:all>
                                    <xs:element name='B' type='xs:float' />
                                    <xs:element name='A' type='xs:unsignedByte' />
                                    <xs:element name='C' type='xs:float' />
                                    <xs:element name='D' type='xs:dateTime' />
                                </xs:all>
                                </xs:complexType>
                            </xs:element>
                            </xs:sequence>
                        </xs:complexType>
                        </xs:element>
                    </xs:schema>", true)]
        [InlineData("noNamespaceSchemaLocation",
                    "<Text><b></b><i></i></Text>",
                    @"<Text xsi:noNamespaceSchemaLocation='http://adventure-works.com/schemas/paragraph.xsd'
                  xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'><b></b><i></i></Text>",
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                       <xsd:element name='Text'>
                        <xsd:complexType>
                         <xsd:all>
                          <xsd:element name='b'/>
                          <xsd:element name='i'/>
                         </xsd:all>
                        </xsd:complexType>
                       </xsd:element>
                      </xsd:schema>", true)]
        [InlineData("hexBinary and language data types",
                    "<Text><b>3f3c6d78206c657673726f693d6e3122302e20226e</b><l>en-US</l></Text>",
                    "<Text><b>3F3C6D78206C657673726F693D6E3122302E20226E</b><l>en-us</l></Text>",
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                       <xsd:element name='Text'>
                        <xsd:complexType>
                         <xsd:all>
                          <xsd:element name='b' type='xsd:hexBinary'/>
                          <xsd:element name='l' type='xsd:language'/>
                         </xsd:all>
                        </xsd:complexType>
                       </xsd:element>
                      </xsd:schema>", true)]
        [InlineData("Attributes of various types, values not normalized.",
                                      @"<Root ABoolean='true'
                          AFloat='1.0'
                          ADecimal='1.00'
                          ADouble='1.0'
                          ADateTime='2009-01-21T18:50:59-08:00'
                          AHexBinary='abcd1234'
                          ALanguage='en-us'>
                      <Child XBoolean='true'
                             XFloat='1.0'
                             XDecimal='1.00'
                             XDouble='1.0'
                             XDateTime='2009-01-21T18:50:59-08:00'
                             XHexBinary='abcd1234'
                             XLanguage='en-us'/>
                    </Root>",
                    @"<Root ABoolean='true'
                            AFloat='+1'
                            ADecimal='1.00'
                            ADouble='+1e+0'
                            ADateTime='2009-01-21T18:50:59.00-08:00'
                            AHexBinary='ABCD1234'
                            ALanguage='EN-US'>
                        <Child XBoolean='true'
                                XFloat='1.0'
                                XDecimal='1.00'
                                XDouble='+1e+0'
                                XDateTime='2009-01-21T18:50:59-08:00'
                                XHexBinary='ABCD1234'
                                XLanguage='en-US'/>
                    </Root>",
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                        <xsd:element name='Root'>
                        <xsd:complexType>
                        <xsd:all>
                        <xsd:element name='Child' minOccurs='1' maxOccurs='1'>
                            <xsd:complexType>
                            <xsd:attribute name='XBoolean' type='xsd:boolean'/>
                            <xsd:attribute name='XFloat' type='xsd:float'/>
                            <xsd:attribute name='XDecimal' type='xsd:decimal'/>
                            <xsd:attribute name='XDouble' type='xsd:double'/>
                            <xsd:attribute name='XDateTime' type='xsd:dateTime'/>
                            <xsd:attribute name='XHexBinary' type='xsd:hexBinary'/>
                            <xsd:attribute name='XLanguage' type='xsd:language'/>
                            </xsd:complexType>
                        </xsd:element>
                        </xsd:all>
                        <xsd:attribute name='ABoolean' type='xsd:boolean'/>
                        <xsd:attribute name='AFloat' type='xsd:float'/>
                        <xsd:attribute name='ADecimal' type='xsd:decimal'/>
                        <xsd:attribute name='ADouble' type='xsd:double'/>
                        <xsd:attribute name='ADateTime' type='xsd:dateTime'/>
                        <xsd:attribute name='AHexBinary' type='xsd:hexBinary'/>
                        <xsd:attribute name='ALanguage' type='xsd:language'/>
                        </xsd:complexType>
                        </xsd:element>
                    </xsd:schema>", true)]
        [InlineData("Element has a default attribute.", 
                    "<Root/>",
                    "<Root ADefaultBooleanAttribute='false'/>",
                    @"<xsd:schema xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                     <xsd:element name='Root'>
                      <xsd:complexType>
                       <xsd:simpleContent>
                        <xsd:extension base='xsd:string'>
                         <xsd:attribute name='ADefaultBooleanAttribute' default='false'/>
                        </xsd:extension>
                       </xsd:simpleContent>
                      </xsd:complexType>
                     </xsd:element>
                    </xsd:schema>", true)]
        public void when_comparing_documents_then_suceeds(string description, string doc1, string doc2, string schema, bool expectedEquals)
        {
            var schemaSet = default(XmlSchemaSet);
            if (schema != null)
            {
                schemaSet = new XmlSchemaSet();
                schemaSet.Add("", XmlReader.Create(new StringReader(schema)));
            }

            var d1 = XDocument.Parse(doc1);
            var d2 = XDocument.Parse(doc2);

            if (expectedEquals == true)
                Assert.True(XmlNormalizer.NormalizedEquals(d1, d2, schemaSet));
            else
                Assert.False(XmlNormalizer.NormalizedEquals(d1, d2, schemaSet));
        }
    }
}