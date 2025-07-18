-- Create customer details table
CREATE TABLE IF NOT EXISTS public.customerdetails
(
    customerid integer NOT NULL,
    firstname character varying(50) COLLATE pg_catalog."default",
    lastname character varying(50) COLLATE pg_catalog."default",
    email character varying(50) COLLATE pg_catalog."default",
    registrationdate date,
    phoneno character varying(15) COLLATE pg_catalog."default",
    CONSTRAINT customerdetails_pkey PRIMARY KEY (customerid)
)
TABLESPACE pg_default;

-- Create inventory table
CREATE TABLE IF NOT EXISTS public.inventory
(
    id integer NOT NULL DEFAULT nextval('inventory_id_seq'::regclass),
    productid integer,
    productname character varying(100) COLLATE pg_catalog."default",
    availableqty integer,
    reorderpoint integer,
    CONSTRAINT inventory_pkey PRIMARY KEY (id)
)
TABLESPACE pg_default;

-- Insert dummy data into customerdetails
INSERT INTO public.customerdetails (customerid, firstname, lastname, email, registrationdate, phoneno) VALUES
(1, 'John', 'Doe', 'john.doe@example.com', '2023-01-15', '1234567890'),
(2, 'Jane', 'Smith', 'jane.smith@example.com', '2023-02-20', '2345678901'),
(3, 'Alice', 'Johnson', 'alice.johnson@example.com', '2023-03-10', '3456789012'),
(4, 'Bob', 'Williams', 'bob.williams@example.com', '2023-04-05', '4567890123'),
(5, 'Charlie', 'Brown', 'charlie.brown@example.com', '2023-05-12', '5678901234');

-- Insert dummy data into inventory
INSERT INTO public.inventory (productid, productname, availableqty, reorderpoint) VALUES
(101, 'Earphones', 50, 10),
(102, 'Mobiles', 30, 5),
(103, 'PlayStation', 100, 20),
(104, 'Earphones', 25, 8),
(105, 'Mobiles', 40, 12);

