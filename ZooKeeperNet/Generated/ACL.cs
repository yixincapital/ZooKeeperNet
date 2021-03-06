// File generated by hadoop record compiler. Do not edit.
/**
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements.  See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership.  The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License.  You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Linq;
using Org.Apache.Jute;
using log4net;

namespace Org.Apache.Zookeeper.Data
{
public class ACL : IRecord, IComparable 
{
private static ILog log = LogManager.GetLogger(typeof(ACL));
  public ACL() {
  }
  public ACL(
  int perms
,
  Org.Apache.Zookeeper.Data.ZKId id
) {
Perms=perms;
Id=id;
  }
  public int Perms { get; set; } 
  public Org.Apache.Zookeeper.Data.ZKId Id { get; set; } 
  public void Serialize(IOutputArchive a_, String tag) {
    a_.StartRecord(this,tag);
    a_.WriteInt(Perms,"perms");
    a_.WriteRecord(Id,"id");
    a_.EndRecord(this,tag);
  }
  public void Deserialize(IInputArchive a_, String tag) {
    a_.StartRecord(tag);
    Perms=a_.ReadInt("perms");
    Id= new Org.Apache.Zookeeper.Data.ZKId();
    a_.ReadRecord(Id,"id");
    a_.EndRecord(tag);
}
  public override String ToString() {
    try {
      System.IO.MemoryStream ms = new System.IO.MemoryStream();
      using(ZooKeeperNet.IO.EndianBinaryWriter writer =
        new ZooKeeperNet.IO.EndianBinaryWriter(ZooKeeperNet.IO.EndianBitConverter.Big, ms, System.Text.Encoding.UTF8)){
      BinaryOutputArchive a_ = 
        new BinaryOutputArchive(writer);
      a_.StartRecord(this,string.Empty);
    a_.WriteInt(Perms,"perms");
    a_.WriteRecord(Id,"id");
      a_.EndRecord(this,string.Empty);
      ms.Position = 0;
      return System.Text.Encoding.UTF8.GetString(ms.ToArray());
    }    } catch (Exception ex) {
      log.Error(ex);
    }
    return "ERROR";
  }
  public void Write(ZooKeeperNet.IO.EndianBinaryWriter writer) {
    BinaryOutputArchive archive = new BinaryOutputArchive(writer);
    Serialize(archive, string.Empty);
  }
  public void ReadFields(ZooKeeperNet.IO.EndianBinaryReader reader) {
    BinaryInputArchive archive = new BinaryInputArchive(reader);
    Deserialize(archive, string.Empty);
  }
  public int CompareTo (object obj) {
    ACL peer = (ACL) obj;
    if (peer == null) {
      throw new InvalidOperationException("Comparing different types of records.");
    }
    int ret = 0;
    ret = (Perms == peer.Perms)? 0 :((Perms<peer.Perms)?-1:1);
    if (ret != 0) return ret;
    ret = Id.CompareTo(peer.Id);
    if (ret != 0) return ret;
     return ret;
  }
  public override bool Equals(object obj) {
 ACL peer = (ACL) obj;
    if (peer == null) {
      return false;
    }
    if (Object.ReferenceEquals(peer,this)) {
      return true;
    }
    bool ret = false;
    ret = (Perms==peer.Perms);
    if (!ret) return ret;
    ret = Id.Equals(peer.Id);
    if (!ret) return ret;
     return ret;
  }
  public override int GetHashCode() {
    int result = 17;
    int ret = GetType().GetHashCode();
    result = 37*result + ret;
    ret = (int)Perms;
    result = 37*result + ret;
    ret = Id.GetHashCode();
    result = 37*result + ret;
    return result;
  }
  public static string Signature() {
    return "LACL(iLId(ss))";
  }
}
}
