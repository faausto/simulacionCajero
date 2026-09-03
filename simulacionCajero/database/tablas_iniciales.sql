CREATE TABLE provincia (
    provincia_id INTEGER PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE ciudades (
    ciudad_id INTEGER PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    codigo_postal VARCHAR(20),
    codigo_telefonico VARCHAR(20),
    provincia_id INTEGER NOT NULL,

    CONSTRAINT fk_ciudad_provincia
        FOREIGN KEY (provincia_id)
        REFERENCES provincia(provincia_id)
);

CREATE TABLE sucursales (
    sucursal_id INTEGER PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    direccion VARCHAR(200),
    telefono VARCHAR(30),
    ciudad_id INTEGER NOT NULL,

    CONSTRAINT fk_sucursal_ciudad
        FOREIGN KEY (ciudad_id)
        REFERENCES ciudades(ciudad_id)
);

CREATE TABLE tipos_cuentas_bancarias (
    tipo_cuenta_id INTEGER PRIMARY KEY,
    nombre VARCHAR(50) NOT NULL,
    permite_sobregiro BOOLEAN NOT NULL
);

CREATE TABLE clientes (
    cuil VARCHAR(11) PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100),
    domicilio VARCHAR(200),
    telefono VARCHAR(30),
    es_persona_fisica BOOLEAN NOT NULL,
    razon_social VARCHAR(150),
    cliente_cuil VARCHAR(11),
    pin VARCHAR(255),
    fecha_nacimiento DATE,
    fecha_alta DATE,

    CONSTRAINT fk_cliente_cliente
        FOREIGN KEY (cliente_cuil)
        REFERENCES clientes(cuil)
);

CREATE TABLE cuentas_bancarias (
    numero_cuenta BIGINT PRIMARY KEY,
    sucursal_id INTEGER NOT NULL,
    tipo_cuenta_id INTEGER NOT NULL,
    monto_sobregiro NUMERIC(15,2),
    es_activa BOOLEAN NOT NULL,
    fecha_alta DATE,

    CONSTRAINT fk_cuenta_sucursal
        FOREIGN KEY (sucursal_id)
        REFERENCES sucursales(sucursal_id),

    CONSTRAINT fk_cuenta_tipo
        FOREIGN KEY (tipo_cuenta_id)
        REFERENCES tipos_cuentas_bancarias(tipo_cuenta_id)
);

CREATE TABLE cuentas_bancarias_cliente (
    numero_cuenta BIGINT,
    cuil VARCHAR(11),

    CONSTRAINT pk_cuentas_bancarias_cliente
        PRIMARY KEY (numero_cuenta, cuil),

    CONSTRAINT fk_cbc_cuenta
        FOREIGN KEY (numero_cuenta)
        REFERENCES cuentas_bancarias(numero_cuenta),

    CONSTRAINT fk_cbc_cuil
        FOREIGN KEY (cuil)
        REFERENCES clientes(cuil)
);

CREATE TABLE tipo_movimientos (
    tipo_movimiento_id INTEGER PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL
);

CREATE TABLE movimientos (
    movimiento_id BIGINT PRIMARY KEY,
    numero_cuenta BIGINT NOT NULL,
    fecha_hora TIMESTAMP NOT NULL,
    monto NUMERIC(15,2) NOT NULL,
    tipo_movimiento_id INTEGER NOT NULL,

    CONSTRAINT fk_movimiento_cuenta
        FOREIGN KEY (numero_cuenta)
        REFERENCES cuentas_bancarias(numero_cuenta),

    CONSTRAINT fk_movimiento_tipo
        FOREIGN KEY (tipo_movimiento_id)
        REFERENCES tipo_movimientos(tipo_movimiento_id)
);

CREATE TABLE transferencia (
    movimiento_id BIGINT PRIMARY KEY,
    numero_cuenta_origen BIGINT NOT NULL,
    numero_cuenta_destino BIGINT NOT NULL,

    CONSTRAINT fk_transferencia_movimiento
        FOREIGN KEY (movimiento_id)
        REFERENCES movimientos(movimiento_id),

    CONSTRAINT fk_transferencia_cuenta_origen
        FOREIGN KEY (numero_cuenta_origen)
        REFERENCES cuentas_bancarias(numero_cuenta),

    CONSTRAINT fk_transferencia_cuenta_destino
        FOREIGN KEY (numero_cuenta_destino)
        REFERENCES cuentas_bancarias(numero_cuenta)
);

CREATE TABLE preguntas (
    pregunta_id INTEGER PRIMARY KEY,
    descripcion TEXT NOT NULL,
    opcion_verdadera VARCHAR(255) NOT NULL,
    opcion_falsa VARCHAR(255) NOT NULL,
    opcion_falsa_bis VARCHAR(255)
);