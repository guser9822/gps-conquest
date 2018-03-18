using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0,0]")]
	public partial class PlayerEntityModelNetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		private int _PlayerLevel;
		public event FieldEvent<int> PlayerLevelChanged;
		public Interpolated<int> PlayerLevelInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int PlayerLevel
		{
			get { return _PlayerLevel; }
			set
			{
				// Don't do anything if the value is the same
				if (_PlayerLevel == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_PlayerLevel = value;
				hasDirtyFields = true;
			}
		}

		public void SetPlayerLevelDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_PlayerLevel(ulong timestep)
		{
			if (PlayerLevelChanged != null) PlayerLevelChanged(_PlayerLevel, timestep);
			if (fieldAltered != null) fieldAltered("PlayerLevel", _PlayerLevel, timestep);
		}
		private bool _IsInGame;
		public event FieldEvent<bool> IsInGameChanged;
		public Interpolated<bool> IsInGameInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool IsInGame
		{
			get { return _IsInGame; }
			set
			{
				// Don't do anything if the value is the same
				if (_IsInGame == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_IsInGame = value;
				hasDirtyFields = true;
			}
		}

		public void SetIsInGameDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_IsInGame(ulong timestep)
		{
			if (IsInGameChanged != null) IsInGameChanged(_IsInGame, timestep);
			if (fieldAltered != null) fieldAltered("IsInGame", _IsInGame, timestep);
		}
		private uint _avatorOwnerNetId;
		public event FieldEvent<uint> avatorOwnerNetIdChanged;
		public Interpolated<uint> avatorOwnerNetIdInterpolation = new Interpolated<uint>() { LerpT = 0f, Enabled = false };
		public uint avatorOwnerNetId
		{
			get { return _avatorOwnerNetId; }
			set
			{
				// Don't do anything if the value is the same
				if (_avatorOwnerNetId == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_avatorOwnerNetId = value;
				hasDirtyFields = true;
			}
		}

		public void SetavatorOwnerNetIdDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_avatorOwnerNetId(ulong timestep)
		{
			if (avatorOwnerNetIdChanged != null) avatorOwnerNetIdChanged(_avatorOwnerNetId, timestep);
			if (fieldAltered != null) fieldAltered("avatorOwnerNetId", _avatorOwnerNetId, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			PlayerLevelInterpolation.current = PlayerLevelInterpolation.target;
			IsInGameInterpolation.current = IsInGameInterpolation.target;
			avatorOwnerNetIdInterpolation.current = avatorOwnerNetIdInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _PlayerLevel);
			UnityObjectMapper.Instance.MapBytes(data, _IsInGame);
			UnityObjectMapper.Instance.MapBytes(data, _avatorOwnerNetId);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_PlayerLevel = UnityObjectMapper.Instance.Map<int>(payload);
			PlayerLevelInterpolation.current = _PlayerLevel;
			PlayerLevelInterpolation.target = _PlayerLevel;
			RunChange_PlayerLevel(timestep);
			_IsInGame = UnityObjectMapper.Instance.Map<bool>(payload);
			IsInGameInterpolation.current = _IsInGame;
			IsInGameInterpolation.target = _IsInGame;
			RunChange_IsInGame(timestep);
			_avatorOwnerNetId = UnityObjectMapper.Instance.Map<uint>(payload);
			avatorOwnerNetIdInterpolation.current = _avatorOwnerNetId;
			avatorOwnerNetIdInterpolation.target = _avatorOwnerNetId;
			RunChange_avatorOwnerNetId(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _PlayerLevel);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _IsInGame);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _avatorOwnerNetId);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (PlayerLevelInterpolation.Enabled)
				{
					PlayerLevelInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					PlayerLevelInterpolation.Timestep = timestep;
				}
				else
				{
					_PlayerLevel = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_PlayerLevel(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (IsInGameInterpolation.Enabled)
				{
					IsInGameInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					IsInGameInterpolation.Timestep = timestep;
				}
				else
				{
					_IsInGame = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_IsInGame(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (avatorOwnerNetIdInterpolation.Enabled)
				{
					avatorOwnerNetIdInterpolation.target = UnityObjectMapper.Instance.Map<uint>(data);
					avatorOwnerNetIdInterpolation.Timestep = timestep;
				}
				else
				{
					_avatorOwnerNetId = UnityObjectMapper.Instance.Map<uint>(data);
					RunChange_avatorOwnerNetId(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (PlayerLevelInterpolation.Enabled && !PlayerLevelInterpolation.current.UnityNear(PlayerLevelInterpolation.target, 0.0015f))
			{
				_PlayerLevel = (int)PlayerLevelInterpolation.Interpolate();
				//RunChange_PlayerLevel(PlayerLevelInterpolation.Timestep);
			}
			if (IsInGameInterpolation.Enabled && !IsInGameInterpolation.current.UnityNear(IsInGameInterpolation.target, 0.0015f))
			{
				_IsInGame = (bool)IsInGameInterpolation.Interpolate();
				//RunChange_IsInGame(IsInGameInterpolation.Timestep);
			}
			if (avatorOwnerNetIdInterpolation.Enabled && !avatorOwnerNetIdInterpolation.current.UnityNear(avatorOwnerNetIdInterpolation.target, 0.0015f))
			{
				_avatorOwnerNetId = (uint)avatorOwnerNetIdInterpolation.Interpolate();
				//RunChange_avatorOwnerNetId(avatorOwnerNetIdInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerEntityModelNetworkObject() : base() { Initialize(); }
		public PlayerEntityModelNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerEntityModelNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
