create table library_schema.Borrowers(
    borrower_id serial primary key ,
    name varchar(100) NOT NULL ,
    email varchar(255) NOT NULL ,
    phone BIGINT NOT NULL
);


create table library_schema.Country(
    country_id serial primary key ,
    name varchar(100) NOT NULL
);


create table library_schema.Authors(
    author_id serial primary key,
    name varchar(100) not null ,
    birth_date date not null ,
    country_id Int NOT NULL,
    constraint fk_country
    foreign key (country_id) references library_schema.Country(country_id)
);

create table library_schema.Books(
    book_id serial primary key,
    title varchar(255) NOT NULL ,
    author_id Int NOT NULL,
    isbn varchar(13),
    published_year date,
    constraint fk_author foreign key (author_id) references library_schema.Authors(author_id)

);

create table library_schema.Loans(
    loan_id serial primary key,
    book_id int not null ,
    constraint fk_book foreign key (book_id) references library_schema.Books(book_id),
    borrower_id int not null,
    constraint fk_borrower foreign key (borrower_id) references library_schema.Borrowers(borrower_id),
    loan_date date not null ,
    return_date date not null ,
    returned bool not null
);

INSERT INTO library_schema.Country (name) VALUES
('United States'),
('United Kingdom'),
('Canada'),
('Australia'),
('Germany');

INSERT INTO library_schema.Authors (name, birth_date, country_id) VALUES
('Mark Twain', '1835-11-30', 1),
('J.K. Rowling', '1965-07-31', 2),
('Margaret Atwood', '1939-11-18', 3),
('Tim Winton', '1960-08-04', 4),
('Johann Wolfgang von Goethe', '1749-08-28', 5);

INSERT INTO library_schema.Borrowers (name, email, phone) VALUES
('Alice Johnson', 'alice.johnson@example.com', 1234567890),
('Bob Smith', 'bob.smith@example.com', 2345678901),
('Carol White', 'carol.white@example.com', 3456789012),
('David Brown', 'david.brown@example.com', 4567890123),
('Eve Black', 'eve.black@example.com', 5678901234);

INSERT INTO library_schema.Books (title, author_id, isbn, published_year) VALUES
('The Adventures of Tom Sawyer', 1, '9780143039563', '1876-06-01'),
('Harry Potter and the Philosophers Stone', 2, '9780747532699', '1997-06-26'),
('The Handmaids Tale', 3, '9780771008795', '1985-09-01'),
('Cloudstreet', 4, '9780140273984', '1991-04-01'),
('Faust', 5, '9780140449014', '1808-01-01');

INSERT INTO library_schema.Loans (book_id, borrower_id, loan_date, return_date, returned) VALUES
(1, 1, '2024-07-01', '2024-07-15', true),
(2, 2, '2024-07-02', '2024-07-16', false),
(3, 3, '2024-07-03', '2024-07-17', true),
(4, 4, '2024-07-04', '2024-07-18', false),
(5, 5, '2024-07-05', '2024-07-19', true);


--Queries
--1
Select * from library_schema.Books where Extract(Year from published_year)='1876';
--2
select * from library_schema.Loans where return_date<current_date and returned=false;
--3
select b2.title  from library_schema.loans l left join library_schema.Borrowers b1
    on l.borrower_id= b1.borrower_id left join library_schema.Books b2
    on l.book_id = b2.book_id where b1.name = 'Bob Smith';
--4
select count(*) from library_schema.books;


--View
create view library_schema.popularBooks as
select count(b.book_id), b.book_id,b.title,b.published_year,b.author_id,b.isbn from
library_schema.Loans l left join library_schema.Books b on l.book_id= b.book_id
group by b.book_id
order by count(b.book_id) Desc
limit 3;

--Procedure
create procedure borrow_book(b_book_id int, b_borrower_id int, b_return_date date)
    language plpgsql
AS $$
BEGIN
    IF EXISTS (
        SELECT 1 FROM library_schema.Books WHERE book_id = b_book_id
    ) THEN
        IF NOT EXISTS (
            SELECT 1 FROM library_schema.Loans WHERE book_id = b_book_id AND returned = FALSE
        ) THEN
            INSERT INTO library_schema.Loans (book_id, borrower_id, loan_date, return_date, returned)
            VALUES (b_book_id, b_borrower_id, CURRENT_DATE, b_return_date, FALSE);
        ELSE
            RAISE EXCEPTION 'Book with ID % is already borrowed', b_book_id;
        END IF;
    ELSE
        RAISE EXCEPTION 'Book with ID % is not in the library', b_book_id;
    END IF;
END;
$$;

CREATE PROCEDURE return_book(r_book_id INT)
LANGUAGE plpgsql
AS $$
BEGIN
    IF exists(select 1 from library_schema.loans where book_id = r_book_id and returned = false) THEN
    UPDATE library_schema.Loans
    SET returned = TRUE
    WHERE loan_id = r_book_id;
    ELSE
    RAISE EXCEPTION 'Loan not found';
    END IF;
END;
$$;