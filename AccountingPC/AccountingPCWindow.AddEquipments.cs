using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void AddPC()
        {
            String commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPC";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text == String.Empty ? null : cpu.Text));
                temp = Convert.ToInt32(cores.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@Cores", temp));
                temp = Convert.ToInt32(frequency.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@Frequency", temp));
                temp = Convert.ToInt32(maxFrequency.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@MaxFrequency", temp));
                temp = Convert.ToInt32(ram.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@RAM", temp));
                temp = Convert.ToInt32(ramFrequency.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@FrequencyRAM", temp));
                temp = Convert.ToInt32(ssd.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@SSD", temp));
                temp = Convert.ToInt32(hdd.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@HDD", temp));
                command.Parameters.Add(new SqlParameter("@Video", vCard.Text == String.Empty ? null : vCard.Text));
                temp = Convert.ToInt32(videoram.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@VRAM", temp));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddNotebook()
        {
            String commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNotebook";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                temp = Convert.ToInt32(cores.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@Cores", temp));
                temp = Convert.ToInt32(frequency.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@Frequency", temp));
                temp = Convert.ToInt32(maxFrequency.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@MaxFrequency", temp));
                temp = Convert.ToInt32(ram.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@RAM", temp));
                temp = Convert.ToInt32(ramFrequency.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@FrequencyRAM", temp));
                temp = Convert.ToInt32(ssd.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@SSD", temp));
                temp = Convert.ToInt32(hdd.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@HDD", temp));
                command.Parameters.Add(new SqlParameter("@Video", vCard.Text == String.Empty ? null : vCard.Text));
                temp = Convert.ToInt32(videoram.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@VRAM", temp));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddMonitor()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddMonitor";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddNetworkSwitch()
        {
            String commandString;
            SqlCommand command;
            int temp;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddNetworkSwitch";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                temp = Convert.ToInt32(ports.Text);
                if (temp > 0)
                    command.Parameters.Add(new SqlParameter("@Ports", temp));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddInteractiveWhiteboard()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddInteractiveWhiteboard";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddPrinterScanner()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddPrinterScanner";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddProjector()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjector";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddProjectorScreen()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddProjectorScreen";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsElectronic", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }

        private void AddOtherEquipment()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "AddOtherEquipment";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?.Row?[0]));
                //command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.ExecuteNonQuery();
            }
        }
    }
}
