using System;
using System.Collections.Generic;

// Interface untuk kemampuan
interface IKemampuan
{
    void Gunakan(Robot pengguna, Robot target);
    bool BisaDigunakan();
}

// Abstract class untuk Robot
abstract class Robot
{
    public string Nama { get; set; }
    public int Energi { get; set; }
    public int Armor { get; set; }
    public int Serangan { get; set; }

    public Robot(string nama, int energi, int armor, int serangan)
    {
        Nama = nama;
        Energi = energi;
        Armor = armor;
        Serangan = serangan;
    }

    // Metode abstrak untuk menyerang
    public abstract void Serang(Robot target);
    public abstract void GunakanKemampuan(IKemampuan kemampuan, Robot target);

    // Menampilkan informasi robot
    public virtual void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }
}

// Kelas RobotTempur
class RobotTempur : Robot
{
    public RobotTempur(string nama) : base(nama, 150, 20, 25)
    {
    }

    public override void Serang(Robot target)
    {
        int damage = Math.Max(0, Serangan - target.Armor);
        target.Energi -= damage;
        Console.WriteLine($"{Nama} menyerang {target.Nama} dengan damage {damage}");
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        if (kemampuan.BisaDigunakan())
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine("Kemampuan sedang cooldown.");
        }
    }
}


// Kelas BosRobot
class BosRobot : Robot
{
    public BosRobot(string nama) : base(nama, 200, 30, 40)
    {
    }

    public override void Serang(Robot target)
    {
        int damage = Math.Max(0, Serangan - target.Armor);
        target.Energi -= damage;
        Console.WriteLine($"{Nama} menyerang {target.Nama} dengan damage {damage}");
    }

    public override void GunakanKemampuan(IKemampuan kemampuan, Robot target)
    {
        if (kemampuan.BisaDigunakan())
        {
            kemampuan.Gunakan(this, target);
        }
        else
        {
            Console.WriteLine("Kemampuan sedang cooldown.");
        }
    }
}

// Kemmampuan Perbaikan
class Perbaikan : IKemampuan
{
    private int cooldown = 0;
    private const int durasiCooldown = 2;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (BisaDigunakan())
        {
            pengguna.Energi += 30;
            Console.WriteLine($"{pengguna.Nama} menggunakan Perbaikan, energi bertambah 30!");
            cooldown = durasiCooldown;
        }
    }

    public bool BisaDigunakan()
    {
        if (cooldown > 0)
        {
            cooldown--;
            return false;
        }
        return true;
    }
}


// Kemampuan SeranganListrik
class SeranganListrik : IKemampuan
{
    private int cooldown = 0;
    private const int durasiCooldown = 3;

    public void Gunakan(Robot pengguna, Robot target)
    {
        if (BisaDigunakan())
        {
            int damage = 40;
            target.Energi -= damage;
            Console.WriteLine($"{pengguna.Nama} menggunakan Serangan Listrik, {target.Nama} terkena 40 damage!");
            cooldown = durasiCooldown;
        }
    }

    public bool BisaDigunakan()
    {
        if (cooldown > 0)
        {
            cooldown--;
            return false;
        }
        return true;
    }
}


// Main Program
class Program
{
    static void Main(string[] args)
    {
        // Pemilihan robot oleh pemain
        Console.WriteLine("Pilih robot Anda:");
        Console.WriteLine("1. Robot Tempur");
        Console.WriteLine("2. Bos Robot");
        string pilihan = Console.ReadLine();

        Robot pemain;
        if (pilihan == "1")
            pemain = new RobotTempur("Robot Alpha");
        else
            pemain = new BosRobot("Bos Omega");

        // Pembuatan lawan
        Robot lawan = new BosRobot("Bos Sigma");

        // Menyediakan kemampuan yang bisa digunakan
        Perbaikan repair = new Perbaikan();
        SeranganListrik listrik = new SeranganListrik();

        // Pertarungan dimulai
        while (pemain.Energi > 0 && lawan.Energi > 0)
        {
            Console.WriteLine("\nGiliran Pemain:");
            pemain.CetakInformasi();
            lawan.CetakInformasi();

            Console.WriteLine("Pilih aksi:");
            Console.WriteLine("1. Serang lawan");
            Console.WriteLine("2. Gunakan Perbaikan");
            Console.WriteLine("3. Gunakan Serangan Listrik");
            string aksi = Console.ReadLine();

            if (aksi == "1")
                pemain.Serang(lawan);
            else if (aksi == "2")
                pemain.GunakanKemampuan(repair, pemain);
            else if (aksi == "3")
                pemain.GunakanKemampuan(listrik, lawan);
            else
                Console.WriteLine("Pilihan tidak valid.");

            // Cek apakah lawan mati
            if (lawan.Energi <= 0)
            {
                Console.WriteLine($"{lawan.Nama} berhasil dikalahkan!");
                break;

            }

            // Giliran lawan menyerang
            Console.WriteLine("\nGiliran Lawan:");
            lawan.Serang(pemain);

            // Cek apakah pemain mati
            if (pemain.Energi <= 0)
            {
                Console.WriteLine($"{pemain.Nama} berhasil dikalahkan!");
                break;
            }
        }

        Console.WriteLine("==Pertarungan selesai==");
    }
}