DROP TABLE TOOLS PURGE;
DROP TABLE STOK_SPAREPART PURGE;
DROP TABLE SPAREPART PURGE;
DROP TABLE TOOLS_CATEGORY PURGE;
DROP TABLE CABANG PURGE;
DROP TABLE SPAREPART_CATEGORY PURGE;
DROP TABLE DTRANS PURGE;
DROP TABLE HTRANS PURGE;
DROP MATERIALIZED VIEW LOCAL_TOOLS;
DROP MATERIALIZED VIEW LOCAL_SPAREPART;

CREATE TABLE TOOLS_CATEGORY(
	ID_KATEGORI VARCHAR2(9) PRIMARY KEY,
	-- CAT/001
	NAMA VARCHAR2(20) NOT NULL
);

CREATE TABLE TOOLS(
	ID_TOOLS VARCHAR2(9) PRIMARY KEY,
	-- AHASS/001
	NAMA VARCHAR2(20) NOT NULL,
	ID_KATEGORI VARCHAR2(9),
	STATUS NUMBER(1) NOT NULL,
	-- 0 = not available, 1 = available
	FOREIGN KEY (ID_KATEGORI) REFERENCES TOOLS_CATEGORY(ID_KATEGORI) 
);

CREATE TABLE CABANG(
	ID_CABANG VARCHAR2(9) PRIMARY KEY,
	NAMA_CABANG VARCHAR2(50) NOT NULL,
	ALAMAT VARCHAR2(50) NOT NULL
);

CREATE TABLE SPAREPART_CATEGORY(
	ID_CATEGORY VARCHAR2(9) PRIMARY KEY,
	CATEGORY_NAME VARCHAR2(20) NOT NULL
);

CREATE TABLE SPAREPART(
	ID_SPARE VARCHAR2(9) PRIMARY KEY,
	NAME VARCHAR2(20) NOT NULL,
	ID_CATEGORY VARCHAR2(20) NOT NULL,
	DESCRIPTION VARCHAR2(50),
	FOREIGN KEY (ID_CATEGORY) REFERENCES SPAREPART_CATEGORY(ID_CATEGORY)
);
-- data sparepart semua cabang
CREATE TABLE STOK_SPAREPART(
	ID VARCHAR2(3) PRIMARY KEY,
	ID_SPARE VARCHAR2(9) NOT NULL,
	ID_CABANG VARCHAR2(9) NOT NULL,
	STOK NUMBER(10) NOT NULL,
	FOREIGN KEY (ID_SPARE) REFERENCES SPAREPART(ID_SPARE),
	FOREIGN KEY (ID_CABANG) REFERENCES CABANG(ID_CABANG)
);

CREATE TABLE HTRANS (
	ID_Transaksi VARCHAR(14) PRIMARY KEY,
	Tanggal DATE,
	NAMA_PEMILIK VARCHAR2(50) NOT NULL,
	ALAMAT_PEMILIK VARCHAR2(50) NOT NULL,
	NO_KTP VARCHAR2(18) NOT NULL,
	NPWP VARCHAR2(20),
	NO_POLISI VARCHAR2(20) NOT NULL,
	DESKRIPSI_KENDARAAN VARCHAR2(50)
);


CREATE TABLE DTRANS (
	id VARCHAR2(9) PRIMARY KEY,
	ID_HTRANS VARCHAR2(14),
	ID_ITEM VARCHAR2(10),
	NAMA_ITEM VARCHAR2(50),
	HARGA_ITEM VARCHAR2(30),
	FOREIGN KEY (ID_HTRANS) REFERENCES HTRANS(ID_Transaksi)
);

CREATE OR REPLACE TRIGGER create_id_tools 
BEFORE INSERT ON TOOLS
FOR EACH ROW
DECLARE
	IDLAMA VARCHAR2(10);
	countid NUMBER(10);
BEGIN
	select MAX(ID_TOOLS) into IDLAMA from TOOLS;
	if (IDLAMA IS NULL) then 
		countid := 1;
	else 
		countid := substr(IDLAMA,-3,3)+1;
	end if;
	:NEW.ID_TOOLS := 'AHASS/' || lpad(countid,3,'0');
END;
/
SHOW ERR;

--sparepart
Create or replace Trigger autoIdSparepart 
before insert 
    on sparepart
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(id_spare) into temp_id from sparepart;
	
	
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,2,3)+1;
	end if;
	:new.id_spare := concat('S',lpad(temp_id,3,'0'));
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;
COMMIT;

--sparepartCategory
Create or replace Trigger autoIdSparepartCategory 
before insert 
    on SPAREPART_CATEGORY
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID_CATEGORY) into temp_id from SPAREPART_CATEGORY;
	
	
	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,2,3)+1;
	end if;
	:new.ID_CATEGORY := concat('SC',lpad(temp_id,3,'0'));
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;
COMMIT;

--STOK_SPAREPART
Create or replace Trigger autoIdStokSparepart
before insert 
    on STOK_SPAREPART
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID) into temp_id from STOK_SPAREPART;

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := temp_id+1;
	end if;

	:new.ID := temp_id;
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;
COMMIT;

--HTRANS
Create or replace Trigger autoIdHtrans
before insert 
    on HTRANS
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID_Transaksi) into temp_id from HTRANS;

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,2,3)+1;
	end if;

	:new.ID_Transaksi := concat('HT',lpad(temp_id,3,'0'));
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;
COMMIT;

--CABANG
Create or replace Trigger autoIdCabang
before insert 
    on CABANG
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID_CABANG) into temp_id from CABANG;

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,2,3)+1;
	end if;

	:new.ID_CABANG := concat('C',lpad(temp_id,3,'0'));
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;
COMMIT;

--TOOLS_CATEGORY
Create or replace Trigger autoIdToolsCategory
before insert 
    on TOOLS_CATEGORY
    for each row
declare 
    temp_id varchar2(10);
    err exception;
begin
	select max(ID_KATEGORI) into temp_id from TOOLS_CATEGORY;

	if temp_id IS NULL then
		temp_id:=1;
	ELSE 
	temp_id := substr(temp_id,2,3)+1;
	end if;

	:new.ID_KATEGORI := concat('CAT',lpad(temp_id,3,'0'));
exception 
    when err then raise_application_error(-20001,'hangus');
END;
/
show err;
COMMIT;

-- AHASS/001

--INSERT INTO TOOLS_CATEGORY VALUES ('','PUMP');
--INSERT INTO TOOLS VALUES ('','PUMP MOTOR YAMAHA','CAT001',1);
--INSERT INTO TOOLS VALUES ('','PUMP MOTOR HARLEY','CAT001',0);
--INSERT INTO SPAREPART_CATEGORY VALUES ('','SPION');
--INSERT INTO SPAREPART VALUES ('','Spion Honda','SC001','Spion dari honda');

--select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.DESCRIPTION FROM SPAREPART S, SPAREPART_CATEGORY SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 
--select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 0 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS T,TOOLS_CATEGORY TC WHERE T.ID_KATEGORI=TC.ID_KATEGORI;
--SELECT * FROM TOOLS;
CREATE MATERIALIZED VIEW LOCAL_TOOLS as select T.ID_TOOLS,T.NAMA,TC.NAMA as Kategori, (CASE WHEN T.STATUS = 1 THEN 'Available' ELSE 'Not Available' END) as Status FROM TOOLS T,TOOLS_CATEGORY TC WHERE T.ID_KATEGORI=TC.ID_KATEGORI;
CREATE MATERIALIZED VIEW LOCAL_SPAREPART as select S.ID_SPARE,S.NAME,SC.CATEGORY_NAME,S.DESCRIPTION FROM SPAREPART S, SPAREPART_CATEGORY SC WHERE S.ID_CATEGORY = SC.ID_CATEGORY; 

-- comment yang punya sendiri
-- field 1 = username
-- field 2 = password 
-- field 3 = tns listener name
-- CREATE PUBLIC DATABASE LINK cabdave CONNECT TO admin IDENTIFIED BY admin USING 'cabdave';