using src.Api.Inputs;
using src.Api.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace src.Database
{
    public class MetadataQueryBuilder
    {


        public static string CreateMetadataString(int? sensorID, string sensorNumber, bool? onlyLast)
        {
            string where = (sensorID != null || sensorNumber != null || onlyLast != null) ? " where " : "";
            if (sensorID != null)
                where = where + " sensor_id= " + sensorID.ToString();
            else if (sensorNumber != null)
                where = where + ((sensorID != null) ? " and " : "") + " number='" + sensorNumber.ToString() + "'";
            if (onlyLast != null && onlyLast == true)
                where = where + ((sensorID != null || sensorNumber != null) ? " and" : "") + " outdated_from IS NULL ";
            string query = $"SELECT * FROM metadata left join location on metadata.location_id=location.id{where} ORDER By created_at desc ";
            return query;
        }
        public static string CreateInsertMetadataString(MetadataInput newMetadata, int? locationID)
        {

            String query = "WITH addedMetadata AS(INSERT INTO metadata(sensor_id, location_id, name, number, company, service_partner, department, " +
                "owner_id, purchase_date, identificator, warranty_date, model_number, serial_number, tag_1, tag_2, tag_3, next_service," +
                " planned_disposal, actual_disposal, lending, lending_price, timeless, check_on_inspectionround, tollerance, cable_length, " +
                "voltage, signal, measure_area, website,picture, inspection_round)" +
                "VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}',{19},{20},{21},{22},{23},{24},'{25}','{26}','{27}','{28}','{29}','{30}') RETURNING *)"
                + "Select addedMetadata.*,coordinate, altitude,description from addedMetadata left join location on (addedMetadata.location_id=location.id);";
            query = string.Format(query, newMetadata.SensorID, locationID, newMetadata.Name, newMetadata.Number, newMetadata.Company,
                newMetadata.ServicePartner, newMetadata.Department, newMetadata.OwnerID, newMetadata.PurchaseDate, newMetadata.Identificator, newMetadata.WarrantyDate,
                newMetadata.ModelNumber, newMetadata.SerialNumber, newMetadata.Tag1, newMetadata.Tag2, newMetadata.Tag3, newMetadata.NextService, newMetadata.PlannedDisposal,
                newMetadata.ActualDisposal, newMetadata.Lending, newMetadata.LendingPrice, newMetadata.Timeless, newMetadata.CheckOnInspectionRound, newMetadata.Tollerance,
                newMetadata.CableLength, newMetadata.Voltage, newMetadata.Signal, newMetadata.MeasureArea, newMetadata.Website, newMetadata.Picture, newMetadata.InspectionRound);
            return query.Replace(",,", ",null,").Replace("''", "null").Replace(",,", ",null,");
        }

        public static string InsertLocationString(MetadataInput newMetadata)
        {
            String query = "INSERT INTO location(description, coordinate, altitude) VALUES('{0}','{1}',{2}) returning id;";
            return String.Format(query, newMetadata.LocationDescription, newMetadata.Coordinate, newMetadata.Altitude);
        }

        public static string UpdateOldMetadataString(DateTime timestamp, int metadataID)
        {
            return "UPDATE metadata SET outdated_from= '" + timestamp + "' WHERE id=" + metadataID + " returning id";
        }
    }
}