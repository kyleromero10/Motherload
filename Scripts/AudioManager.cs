using Godot;
using System.Collections.Generic;

public partial class AudioManager : Node
{
	public static AudioManager I { get; private set; }
	
	private double _gunpowderPlaceNextTime = 0.0;
	[Export] public float GunpowderPlaceCooldownSec { get; set; } = 0.08f; // ~12.5 plays/sec

	// Drag your BgmPlayer node here in Inspector (or it auto-finds by name).
	[Export] public AudioStreamPlayer BgmPlayer { get; set; }

	[Export] public int SfxPoolSize { get; set; } = 16; // good for chain explosions
	[Export] public float SfxVolumeDb { get; set; } = 0f;
	[Export] public float BgmVolumeDb { get; set; } = -6f;

	// SFX clips (drag audio files into these)
	[Export] public AudioStream ButtonPress { get; set; }
	[Export] public AudioStream BombPlace { get; set; }
	[Export] public AudioStream GunpowderPlace { get; set; }
	[Export] public AudioStream SmallBombExplode { get; set; }
	[Export] public AudioStream LargeBombExplode { get; set; }
	[Export] public AudioStream GunpowderLit { get; set; }
	[Export] public AudioStream GoldCollect { get; set; }
	[Export] public AudioStream Demerit { get; set; }
	[Export] public AudioStream VictorySfx { get; set; }

	// Music clip
	[Export] public AudioStream StageBgm { get; set; }

	private readonly List<AudioStreamPlayer> _sfxPlayers = new();
	private int _sfxIndex = 0;

	public override void _EnterTree()
	{
		if (I != null && I != this)
		{
			QueueFree();
			return;
		}
		I = this;
	}

	public override void _Ready()
	{
		BgmPlayer ??= GetNodeOrNull<AudioStreamPlayer>("BgmPlayer");
		if (BgmPlayer != null)
		{
			BgmPlayer.Bus = "Music";
			BgmPlayer.VolumeDb = BgmVolumeDb;
		}

		BuildSfxPool();
	}

	private void BuildSfxPool()
	{
		var poolParent = GetNodeOrNull<Node>("SfxPool") ?? this;

		foreach (var p in _sfxPlayers)
			p.QueueFree();
		_sfxPlayers.Clear();

		for (int i = 0; i < SfxPoolSize; i++)
		{
			var p = new AudioStreamPlayer
			{
				Bus = "SFX",
				VolumeDb = SfxVolumeDb
			};
			poolParent.AddChild(p);
			_sfxPlayers.Add(p);
		}
	}

	private void PlaySfx(AudioStream stream, float volumeDb = 0f, float pitchScale = 1f)
	{
		if (stream == null || _sfxPlayers.Count == 0)
			return;

		var p = _sfxPlayers[_sfxIndex];
		_sfxIndex = (_sfxIndex + 1) % _sfxPlayers.Count;

		p.Stop();
		p.Stream = stream;
		p.VolumeDb = SfxVolumeDb + volumeDb;
		p.PitchScale = pitchScale;
		p.Play();
	}

	public void PlayUiClick() => PlaySfx(ButtonPress);
	public void PlayBombPlace() => PlaySfx(BombPlace);
	public void PlayGunpowderPlace()
{
	double now = Time.GetTicksMsec() / 1000.0;
	if (now < _gunpowderPlaceNextTime)
		return;

	_gunpowderPlaceNextTime = now + GunpowderPlaceCooldownSec;
	PlaySfx(GunpowderPlace);
}
	public void PlaySmallExplode() => PlaySfx(SmallBombExplode);
	public void PlayLargeExplode() => PlaySfx(LargeBombExplode);
	public void PlayGunpowderLit() => PlaySfx(GunpowderLit);
	public void PlayGoldCollect() => PlaySfx(GoldCollect);
	public void PlayDemerit() => PlaySfx(Demerit);
	public void PlayVictory() => PlaySfx(VictorySfx);

	public void PlayStageBgm()
	{
		if (BgmPlayer == null || StageBgm == null)
			return;

		if (BgmPlayer.Stream != StageBgm)
			BgmPlayer.Stream = StageBgm;

		BgmPlayer.VolumeDb = BgmVolumeDb;

		if (!BgmPlayer.Playing)
			BgmPlayer.Play();
	}

	public void StopBgm()
	{
		if (BgmPlayer == null) return;
		BgmPlayer.Stop();
	}
}
