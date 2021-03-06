using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;

[System.Serializable]
public class ABMD5
{
    [XmlElement("ABMD5List")]
    public List<ABMD5Base> ABMD5List { get; set; }
}

[System.Serializable]
public class ABMD5Base
{
    [XmlAttribute("Name")]
    public string Name { get; set; }
    [XmlAttribute("Md5")]
    public string Md5 { get; set; }
    [XmlAttribute("Size")]
    public float Size { get; set; }
}
