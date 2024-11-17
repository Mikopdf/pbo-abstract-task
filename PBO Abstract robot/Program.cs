using System;
using System.Collections.Generic;


public interface IKemampuan
{
    string Nama { get; }
    int Cooldown { get; }
    void Gunakan(Robot target);
}


public abstract class Robot
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

    public void Serang(Robot target)
    {
        int damage = Math.Max(0, Serangan - target.Armor);
        target.Energi -= damage;
        Console.WriteLine($"{Nama} menyerang {target.Nama}, memberikan {damage} damage!");
    }

    public abstract void GunakanKemampuan(IKemampuan kemampuan);

    public void CetakInformasi()
    {
        Console.WriteLine($"Nama: {Nama}, Energi: {Energi}, Armor: {Armor}, Serangan: {Serangan}");
    }


    public void PulihkanEnergi()
    {
        Energi += 10;
        Console.WriteLine($"{Nama} memulihkan 10 energi.");
    }

    public bool IsMati()
    {
        return Energi <= 0;
    }
}


public class BosRobot : Robot
{
    public int Pertahanan { get; set; }

    public BosRobot(string nama, int energi, int armor, int serangan, int pertahanan)
        : base(nama, energi, armor, serangan)
    {
        Pertahanan = pertahanan;
    }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        Console.WriteLine("Bos Robot tidak bisa menggunakan kemampuan khusus.");
    }

    public void Diserang(Robot penyerang)
    {
        int damage = Math.Max(0, penyerang.Serangan - Pertahanan);
        Energi -= damage;
        Console.WriteLine($"{penyerang.Nama} menyerang {Nama} dengan {damage} damage!");
        if (Energi <= 0) Mati();
    }

    public void Mati()
    {
        Console.WriteLine($"{Nama} telah hancur!");
    }
}


public class Perbaikan : IKemampuan
{
    public string Nama => "Perbaikan";
    public int Cooldown { get; private set; } = 3;

    public void Gunakan(Robot target)
    {
        int pemulihanEnergi = 30;
        target.Energi += pemulihanEnergi;
        Console.WriteLine($"{target.Nama} menggunakan {Nama}, memulihkan {pemulihanEnergi} energi.");
    }
}

public class SeranganListrik : IKemampuan
{
    public string Nama => "Serangan Listrik";
    public int Cooldown { get; private set; } = 5;

    public void Gunakan(Robot target)
    {
        int damage = 20;
        target.Energi -= damage;
        Console.WriteLine($"{target.Nama} terkena {Nama} dan menerima {damage} damage!");
    }
}

public class MeriamPlasma : IKemampuan
{
    public string Nama => "Meriam Plasma";
    public int Cooldown { get; private set; } = 4;

    public void Gunakan(Robot target)
    {
        int damage = 40;
        target.Energi -= damage;
        Console.WriteLine($"{target.Nama} terkena {Nama} dan menerima {damage} damage!");
    }
}

public class PerisaiSuper : IKemampuan
{
    public string Nama => "Perisai Super";
    public int Cooldown { get; private set; } = 6;

    public void Gunakan(Robot target)
    {
        int peningkatanArmor = 20;
        target.Armor += peningkatanArmor;
        Console.WriteLine($"{target.Nama} menggunakan {Nama} dan meningkatkan armornya sebesar {peningkatanArmor}!");
    }
}


public class RobotTempur : Robot
{
    public RobotTempur(string nama, int energi, int armor, int serangan)
        : base(nama, energi, armor, serangan) { }

    public override void GunakanKemampuan(IKemampuan kemampuan)
    {
        Console.WriteLine($"{Nama} menggunakan {kemampuan.Nama}!");
        kemampuan.Gunakan(this);
    }
}


class Program
{
    static void Main(string[] args)
    {
        Robot robot1 = new RobotTempur("Robot ambalingham", 100, 20, 30);
        Robot robot2 = new RobotTempur("Robot rusdicop", 250, 30, 25);
        BosRobot boss = new BosRobot("Bos ambatimus prime", 200, 50, 20, 40);

        IKemampuan perbaikan = new Perbaikan();
        IKemampuan seranganListrik = new SeranganListrik();
        IKemampuan meriamPlasma = new MeriamPlasma();
        IKemampuan perisaiSuper = new PerisaiSuper();

        Console.WriteLine("=== Mulai Pertarungan! ===\n");

        robot1.CetakInformasi();
        robot2.CetakInformasi();
        boss.CetakInformasi();

        int giliran = 1;
        string input;

        while (!robot1.IsMati() && !robot2.IsMati() && !boss.IsMati())
        {
            Console.WriteLine($"\n--- Giliran {giliran} ---");
            Console.WriteLine("Tekan 1 untuk menjalankan giliran sampai salah satu robot mati...");
            input = Console.ReadLine();

            if (input == "1")
            {
                robot1.Serang(robot2);

                robot2.GunakanKemampuan(seranganListrik);

                boss.Diserang(robot1);
                boss.Diserang(robot2);

                robot1.PulihkanEnergi();
                robot2.PulihkanEnergi();
                boss.PulihkanEnergi();

                robot1.CetakInformasi();
                robot2.CetakInformasi();
                boss.CetakInformasi();

                giliran++;
            }
            else
            {
                Console.WriteLine("Input tidak valid. Tekan 1 untuk melanjutkan.");
            }
        }

        Console.WriteLine("\n=== Pertarungan Berakhir ===");

        if (robot1.IsMati()) Console.WriteLine($"{robot1.Nama} kalah!");
        if (robot2.IsMati()) Console.WriteLine($"{robot2.Nama} kalah!");
        if (boss.IsMati()) Console.WriteLine($"{boss.Nama} kalah!");
    }
}
