  # Shell provisioner.
  puts "Start installing Ansible"
  $script = <<-SCRIPT
    apt-get update
    apt-get install software-properties-common
    apt-add-repository ppa:ansible/ansible
    apt-get update
    apt-get install -y ansible
  SCRIPT
  config.vm.provision "shell", inline: $script
  puts "Installed Ansible"
  config.vm.provision "shell", inline: "ansible-galaxy install -r /vagrant/requirements.yml"

  config.vm.provision "shell", inline: "apt-get install -y openssh-server"
  config.vm.provision "shell", inline: "cd /vagrant/provisioning; ansible-playbook playbook.yml"

  # Ansible provisioner.
  # config.vm.provision "ansible" do |ansible|
  #   ansible.compatibility_mode = "2.0"
  #   ansible.playbook = "provisioning/playbook.yml"
  #   ansible.inventory_path = "provisioning/inventory"
  #   ansible.become = true
  # end
