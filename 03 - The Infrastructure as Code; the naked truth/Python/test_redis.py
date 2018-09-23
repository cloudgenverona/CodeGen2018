### Test Runner command
# testinfra -v test_redis.py

def test_redis_is_installed(Package):
    redis = Package("redis-server")
    assert redis.is_installed


def test_config_file(File):
    config_file = File('/etc/redis/redis.conf')
    assert config_file.contains('bind 127.0.0.1')  # todo make it a regex
    assert config_file.is_file


def test_service_running(Service):
    service = Service('redis-server')
    assert service.is_running


def test_socket_listening(Socket):
    socket = Socket('tcp://127.0.0.1:6379')
    assert socket.is_listening


def test_command_output(Command):
    command = Command('redis-cli ping')
    assert command.stdout.rstrip() == 'PONG'
    assert command.rc == 0