drop table if exists user_group cascade;
CREATE TABLE user_group (
	id serial primary key,
	name text,
	description text
);

drop table if exists user_in_user_group cascade;
CREATE TABLE user_in_user_group (
	user_group_id integer not null
		references user_group(id)
		on delete cascade
		on update cascade,
	user_id text not null
);

drop table if exists sensor cascade;
create table sensor(
	id serial primary key,
	table_name text,
	column_name text
);

/*drop table if exists sensor_responsable cascade;
create table sensor_responsable(
	sensor_id integer 
		references sensor(id)
		on delete cascade
		on update cascade,
	user_id text
);
All engineers are responsable of all sensors in this prototype
*/
drop table if exists sensor_group cascade;
create table sensor_group(
	id serial primary key,
	details text
);

drop table if exists sensor_in_sensor_group cascade;
create table sensor_in_sensor_group(
	sensor_id integer not null 
		references sensor(id)
		on delete cascade
		on update cascade,
	sensor_group_id integer not null
		references sensor_group(id)
		on delete cascade
		on update cascade
);



drop table if exists location cascade;
create table location(
	id serial primary key,
	description text,
	coordinate text,
	altitude numeric,
	created timestamp not null default now()
);



drop table if exists metadata cascade;
create table metadata(
	id serial primary key,
	sensor_id integer not null
		references sensor(id) 
		on update cascade 
		on delete cascade,
	location_id integer 
		references location(id) 
		on update cascade 
		on delete cascade,
	name text not null,
	number text not null,
	company text,
	service_partner text,
	department text,
	owner_id text,
	purchase_date date,
	identificator text,
	warranty_date date,
	model_number text,
	serial_number text,
	tag_1 text,
	tag_2 text,
	tag_3 text,
	next_service date,
	planned_disposal date,
	actual_disposal date,
	lending boolean,
	lending_price numeric,
	timeless boolean,
	check_on_inspectionround boolean,
	tollerance boolean,
	picture text,
	cable_length numeric,
	voltage text,
	signal text,
	measure_area text, 
	website text,
	inspection_round text,
	created_at timestamp(0) not null default NOW(),
	updated_at timestamp(0) not null default NOW(),
	outdated_from timestamp(0)
);

CREATE OR REPLACE FUNCTION trigger_update_timestamp()
RETURNS TRIGGER AS $$
BEGIN
  NEW.updated_at = NOW();
  RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER update_timestamp
BEFORE UPDATE ON metadata
FOR EACH ROW
EXECUTE PROCEDURE trigger_update_timestamp();
	
drop table if exists log cascade;
create table log(
	id serial primary key,
	metadata_id integer not null
		references metadata(id)
		on update cascade
		on delete cascade,
	author_id text not null,
	title text,
	text text,
	date timestamp not null,
	attachment json
);

drop table if exists user_group_access_to_sensor cascade;
create table user_group_access_to_sensor(
	user_group_id integer not null 
		references user_group(id)
		on delete cascade
		on update cascade,
	sensor_id integer not null
		references sensor(id)
		on delete cascade
		on update cascade,
	access_level text
);

drop table if exists user_group_access_to_sensor_group cascade;
create table user_group_access_to_sensor_group(
	user_group_id integer not null 
		references user_group(id)
		on delete cascade
		on update cascade,
	sensor_group_id integer not null
		references sensor_group(id)
		on delete cascade
		on update cascade,
	access_level text
);

drop table if exists user_access_to_sensor cascade;
create table user_access_to_sensor(
	user_id text,
	sensor_id integer not null
		references sensor(id)
		on delete cascade
		on update cascade,
	access_level text
);

drop table if exists user_access_to_sensor_group cascade;
create table user_access_to_sensor_group(
	user_id text not null,
	sensor_group_id integer not null
		references sensor_group(id)
		on delete cascade
		on update cascade,
	access_level text
);

drop table if exists dashboard cascade;
create table dashboard(
	id serial primary key,
	name text,
	description text);


drop table if exists cell cascade;
create table cell(
    id serial primary key,
	dashboard_id integer not null
		references dashboard(id)
		on delete cascade
		on update cascade,
    input json not null,
    options json
    );

drop table if exists user_access_to_dashboard cascade;
create table user_access_to_dashboard(
	user_id text not null,
	dashboard_id integer not null
		references dashboard(id)
		on delete cascade
		on update cascade,
	access_level text
);

drop table if exists user_group_access_to_dashboard cascade;
create table user_group_access_to_dashboard(
	user_group_id integer not null
		references dashboard(id)
		on delete cascade
		on update cascade,
	dashboard_id integer not null
		references dashboard(id)
		on delete cascade
		on update cascade,
	access_level text
);

drop table if exists experiment cascade;
create table experiment(
	id serial primary key,
	start_time timestamp,
	finish_time timestamp,
	description text
);

drop table if exists sensor_experiment cascade;
create table sensor_experiment(
	experiment_id integer not null
		references experiment(id)
		on delete cascade
		on update cascade,
	sensor_id integer not null
		references sensor(id)
		on delete cascade
		on update cascade,
	start_time timestamp,
	finish_time timestamp
);

drop table if exists user_experiment cascade;
create table user_experiment(
	experiment_id integer not null
		references experiment(id)
		on delete cascade
		on update cascade,
	user_id text not null,
	access_level text
);

drop table if exists user_group_experiment cascade;
create table user_group_experiment(
	experiment_id integer not null
		references experiment(id)
		on delete cascade
		on update cascade,
	user_group_id integer not null
		references user_group(id)
		on delete cascade
		on update cascade,
	access_level text
);
