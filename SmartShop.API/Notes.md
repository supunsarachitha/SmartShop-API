

-- Update ef core
Add-Migration InitialCreate2
Update-Database

--swagger
https://localhost:{port}/swagger

-- use userDto in user controller because it is more secure