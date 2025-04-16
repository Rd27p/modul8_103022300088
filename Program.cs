using System;
using System.IO;
using System.Text;
using System.Text.Json;

class BankTransferConfig
{
    public string lang { get; set; }
    public transferclass transfer { get; set; }
    public List<string> methods { get; set; }
    public confirmationclass confirmation { get; set; }

    private string configfile = @"bank_transfer_config.json";

    public void loadConfig()
    {
        try
        {
            string jsonString = File.ReadAllText(configfile);
            BankTransferConfig config = JsonSerializer.Deserialize<BankTransferConfig>(jsonString);
            lang = config.lang;
            transfer = config.transfer;
            methods = config.methods;
            confirmation = config.confirmation;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading configuration: " + ex.Message);
        }
    }
    public void saveConfig()
    {
        try
        {
            string jsonString = JsonSerializer.Serialize(this);
            File.WriteAllText(configfile, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving configuration: " + ex.Message);
        }
    }


    public class transferclass
    {
        public int threshold { get; set; }
        public int low_fee { get; set; }
        public int high_fee { get; set; }
    }

    public class confirmationclass
    {
        public string en { get; set; }
        public string id { get; set; }
    }

    public void ubahBahasa()
    {
        if (lang == "en")
        {
            lang = "id";
        }
        else
        {
            lang = "en";

        }
    }

    public int getTransferFee(int amount)
    {
        if (amount <= transfer.threshold)
        {
            return transfer.low_fee;
        }
        else
        {
            return transfer.high_fee;
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var config = new BankTransferConfig();
        config.loadConfig();

        Console.WriteLine("Silahkan Pilih Bahasa (en/id) ");
        string lang = Console.ReadLine();
        if (lang == "en")
        {
            config.lang = "en";
        }
        else if (lang == "id")
        {
            config.lang = "id";
        }
        else
        {
            Console.WriteLine("Bahasa tidak valid, menggunakan bahasa default (en)");
            config.lang = "en";
        }

        if (config.lang == "en")
        {
            Console.WriteLine("Please enter the transfer: ");
            config.transfer.threshold = int.Parse(Console.ReadLine());
        }
        else
        {
            Console.WriteLine("Silahkan masukkan transfer: ");
            config.transfer.threshold = int.Parse(Console.ReadLine());
        }

       int biayatransefer = (config.getTransferFee(config.transfer.threshold));
        Console.WriteLine("Biaya transfer: " + biayatransefer);
        Console.WriteLine("Konfirmasi transfer (en/id): ");
        string konfirmasi = Console.ReadLine();
        if (konfirmasi == "en")
        {
            Console.WriteLine(config.confirmation.en);
        }
        else if (konfirmasi == "id")
        {
            Console.WriteLine(config.confirmation.id);
        }
        else
        {
            Console.WriteLine("Bahasa tidak valid, menggunakan bahasa default (en)");
            Console.WriteLine(config.confirmation.en);
        }
        config.saveConfig();
    }
}