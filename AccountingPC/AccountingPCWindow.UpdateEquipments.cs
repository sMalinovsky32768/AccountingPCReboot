using System;
using System.Data;
using System.Data.SqlClient;

namespace AccountingPC
{
    public partial class AccountingPCWindow
    {
        private void UpdatePC()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePCByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MB", motherboard.Text));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateNotebook()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNotebookByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Type", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@CPU", cpu.Text));
                command.Parameters.Add(new SqlParameter("@Cores", Convert.ToInt32(cores.Text)));
                command.Parameters.Add(new SqlParameter("@Frequency", Convert.ToInt32(frequency.Text)));
                command.Parameters.Add(new SqlParameter("@MaxFrequency", Convert.ToInt32(maxFrequency.Text)));
                command.Parameters.Add(new SqlParameter("@RAM", Convert.ToInt32(ram.Text)));
                command.Parameters.Add(new SqlParameter("@FrequencyRAM", Convert.ToInt32(ramFrequency.Text)));
                command.Parameters.Add(new SqlParameter("@SSD", Convert.ToInt32(ssd.Text)));
                command.Parameters.Add(new SqlParameter("@HDD", Convert.ToInt32(hdd.Text)));
                command.Parameters.Add(new SqlParameter("@Video", vCard.Text));
                command.Parameters.Add(new SqlParameter("@VRAM", Convert.ToInt32(videoram.Text)));
                command.Parameters.Add(new SqlParameter("@OSID", ((DataRowView)os?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateMonitor()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateMonitorByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@FrequencyID", ((DataRowView)screenFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@MatrixID", ((DataRowView)matrixTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateNetworkSwitch()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateNetworkSwitchByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@NumberOfPorts", Convert.ToInt32(ports.Text)));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Frequency", ((DataRowView)wifiFrequency?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateInteractiveWhiteboard()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateInteractiveWhiteboardByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdatePrinterScanner()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdatePrinterScannerByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@TypeID", ((DataRowView)type?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@PaperSizeID", ((DataRowView)paperSize?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateProjector()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@TechnologyID", ((DataRowView)projectorTechnology?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@ResolutionID", ((DataRowView)resolution?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@VConnectors", GetValueVideoConnectors(vConnectors)));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateProjectorScreen()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateProjectorScreenByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Diagonal", Convert.ToSingle(diagonal.Text)));
                command.Parameters.Add(new SqlParameter("@IsEDrive", Convert.ToBoolean(isEDrive.IsChecked)));
                command.Parameters.Add(new SqlParameter("@AspectRatioID", ((DataRowView)aspectRatio?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@InstalledID", ((DataRowView)screenInstalled?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }

        private void UpdateOtherEquipment()
        {
            String commandString;
            SqlCommand command;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                commandString = "UpdateOtherEquipmentByID";
                command = new SqlCommand(commandString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@ID", DeviceID));
                command.Parameters.Add(new SqlParameter("@InvN", Convert.ToInt32(inventoryNumber.Text)));
                command.Parameters.Add(new SqlParameter("@Name", name.Text));
                command.Parameters.Add(new SqlParameter("@Cost", Convert.ToSingle(cost.Text)));
                command.Parameters.Add(new SqlParameter("@InvoiceNumber", invoice.Text == String.Empty ? null : invoice.Text));
                command.Parameters.Add(new SqlParameter("@PlaceID", ((DataRowView)location?.SelectedItem)?[0]));
                command.Parameters.Add(new SqlParameter("@Image", LoadImage(imageFilename.Text)));
                command.Parameters.Add(new SqlParameter("@IsChangeAnalog", IsChangeAnalog));
                command.ExecuteNonQuery();
            }
        }
    }
}
