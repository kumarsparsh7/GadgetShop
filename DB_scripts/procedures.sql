-- Insert customer details
CREATE OR REPLACE PROCEDURE public.sp_savecustomerdetails(
	IN customerid integer,
	IN firstname character varying,
	IN lastname character varying,
	IN email character varying,
	IN registrationdate date,
	IN phoneno character varying)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	INSERT INTO customerdetails (
		customerid,
		firstname,
		lastname,
		email,
		registrationdate,
		phoneno
	) VALUES (
		customerid,
		firstname,
		lastname,
		email,
		registrationdate,
		phoneno
	);
END;
$BODY$;

-- Insert inventory data
CREATE OR REPLACE PROCEDURE public.sp_saveinventorydata(
	IN productid integer,
	IN productname character varying,
	IN availableqty integer,
	IN reorderpoint integer)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	INSERT INTO Inventory (
		ProductId,
		ProductName,
		AvailableQty,
		ReorderPoint
	) VALUES (
		ProductId,
		ProductName,
		AvailableQty,
		ReorderPoint
	);
END;
$BODY$;

-- Get customer details
CREATE OR REPLACE PROCEDURE public.sp_getcustomerdetails()
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	SELECT customerid,
	firstname,
	lastname,
	email,
	phoneno,
	CAST(registrationdate as varchar(12)) 
	FROM customerDetails;
END;
$BODY$;

-- Get inventory data
CREATE OR REPLACE PROCEDURE public.sp_getinventorydata()
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	SELECT * FROM inventory;
END;
$BODY$;

-- Delete customer details
CREATE OR REPLACE PROCEDURE public.sp_deletecustomerdetails(
	IN cid integer)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	DELETE FROM customerdetails
	WHERE customerdetails.customerid=cid;
END;
$BODY$;

-- Delete inventory data
CREATE OR REPLACE PROCEDURE public.sp_deleteinventorydata(
	IN pid integer)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	DELETE FROM inventory
	WHERE inventory.productid=pid;
END;
$BODY$;

-- Update customer details
CREATE OR REPLACE PROCEDURE public.sp_updatecustomerdetails(
	IN _customerid integer, 
	IN _firstname character varying, 
	IN _lastname character varying, 
	IN _email character varying, 
	IN _registrationdate date, 
	IN _phoneno character varying)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	Update customerdetails SET 
		firstname = _firstname,
		lastname = _lastname,
		email = _email,
		registrationdate = _registrationdate,
		phoneno = _phoneno
		WHERE customerid = _customerid;
END;
$BODY$;

-- Update inventory data
CREATE OR REPLACE PROCEDURE public.sp_updateinventorydata(
	IN _productid integer,
	IN _productname character varying,
	IN _availableqty integer,
	IN _reorderpoint integer)
LANGUAGE 'plpgsql'
AS $BODY$
BEGIN
	Update Inventory SET 
		ProductName = _productname,
		AvailableQty = _availableqty,
		ReorderPoint = _reorderpoint
		WHERE ProductId = _productid;
END;
$BODY$;