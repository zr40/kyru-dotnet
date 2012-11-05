using ProtoBuf;

namespace Kyru.Network.Objects
{
	/// <summary>
	/// KyruObject is an abstraction for all objects stored in the Kyru network. It determines the type of the object.
	/// </summary>
	[ProtoContract, ProtoInclude(1, typeof(User)), ProtoInclude(2, typeof(Chunk))]
	internal abstract class KyruObject
	{
		/// <remarks>Not part of the serialized object</remarks>
		internal KademliaId ObjectId;

		internal User User
		{
			get
			{
				return this as User;
			}
		}

		internal Chunk Chunk
		{
			get
			{
				return this as Chunk;
			}
		}

		internal bool VerifyData()
		{
			// TODO
			return true;
		}
	}
}