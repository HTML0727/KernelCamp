# Native Library Makefile

CC = gcc
CFLAGS = -Wall -fPIC -shared -I.
LDFLAGS = -lm
TARGET = libkernel.so
SOURCES = kernel.c
OBJECTS = $(SOURCES:.c=.o)

.PHONY: all clean install

all: $(TARGET)

$(TARGET): $(OBJECTS)
	$(CC) $(CFLAGS) -o $@ $^ $(LDFLAGS)

%.o: %.c
	$(CC) $(CFLAGS) -c $< -o $@

clean:
	rm -f $(TARGET) $(OBJECTS)

install: $(TARGET)
	cp $(TARGET) /usr/local/lib/
	ldconfig

test: $(TARGET)
	@echo "Running basic tests..."
	@echo "Kernel version: $$(./test_version)"

# 简单的测试程序
test_version: test_version.c
	$(CC) -o $@ $< -L. -lkernel