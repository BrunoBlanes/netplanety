﻿namespace Netplanety.Integrations.IXC.Tests.Mocks;

internal readonly struct MockApiResponse
{
    internal const string FiberClient_7314 = """{"page":"1","total":"1","registros":[{"id_projeto":"26","gemport":"","ip_gerencia":"","login_onu_cliente":"","senha_onu_cliente":"","porta_telnet_onu_cliente":"","perfil_onu_cliente":"0","script_onu_cliente":"","senorid":"","latitude":"-20.123450633114","longitude":"-40.322125313527","endereco_padrao_cliente":"S","id_condominio":"0","bloco":"","apartamento":"0","cep":"","endereco":"","numero":"","bairro":"","cidade":"0","referencia":"","complemento":"","distancia_onu":"1473","vlan_dhcp":"","vlan_tr69":"","vlan_iptv":"","vlan_voip":"","vlan_pppoe":"","vlan_outros":"","id_ramal":"0","id_onu_unms":"","id_activity":"","radpop_estrutura":"N","porta_web_onu_cliente":"0","tipo_operacao":"","id":"7314","id_transmissor":"3","nome":"12707_BRUNO_ORTEGA_BLANES\r","id_caixa_ftth":"16","porta_ftth":"13","id_login":"9171","id_contrato":"10085","onu_numero":"47","service_port":"","onu_tipo":"","ponid":"0\/1\/5","comandos":"interface gpon #subrack#\/#slot#; ont add #pon# sn-auth #onu_mac# omci ont-lineprofile-id #vlan# ont-srvprofile-id 200 desc #nome#; ont port native-vlan #pon# #onu_numero# eth 1 vlan #vlan#; quit; service-port vlan #vlan# gpon #pon_id# ont #onu_numero# gemport 10 multi-service user-vlan #vlan#;","mac":"4857544376C0E3A5","sinal_rx":"21.35","sinal_tx":"25.41","temperatura":"52.00","voltagem":"3.32","data_sinal":"2024-11-27 15:22:00","id_perfil":"1","slotno":"1","ponno":"5","tipo_autenticacao":"MAC","versao":"","vlan":"1000","causa_ultima_queda":"LOSi\/LOBi","onu_rede_neutra":"N","id_hardware":"0"}]}""";

    internal const string FiberClient_Duplicate_Id = """{"page":"1","total":"1","registros":[{"id_projeto":"26","gemport":"","ip_gerencia":"","login_onu_cliente":"","senha_onu_cliente":"","porta_telnet_onu_cliente":"","perfil_onu_cliente":"0","script_onu_cliente":"","senorid":"","latitude":"-20.123450633114","longitude":"-40.322125313527","endereco_padrao_cliente":"S","id_condominio":"0","bloco":"","apartamento":"0","cep":"","endereco":"","numero":"","bairro":"","cidade":"0","referencia":"","complemento":"","distancia_onu":"1473","vlan_dhcp":"","vlan_tr69":"","vlan_iptv":"","vlan_voip":"","vlan_pppoe":"","vlan_outros":"","id_ramal":"0","id_onu_unms":"","id_activity":"","radpop_estrutura":"N","porta_web_onu_cliente":"0","tipo_operacao":"","id":"7314","id_transmissor":"3","nome":"12707_BRUNO_ORTEGA_BLANES\r","id_caixa_ftth":"16","porta_ftth":"13","id_login":"9171","id_contrato":"10085","onu_numero":"47","service_port":"","onu_tipo":"","ponid":"0\/1\/5","comandos":"interface gpon #subrack#\/#slot#; ont add #pon# sn-auth #onu_mac# omci ont-lineprofile-id #vlan# ont-srvprofile-id 200 desc #nome#; ont port native-vlan #pon# #onu_numero# eth 1 vlan #vlan#; quit; service-port vlan #vlan# gpon #pon_id# ont #onu_numero# gemport 10 multi-service user-vlan #vlan#;","mac":"4857544376C0E3A5","sinal_rx":"21.35","sinal_tx":"25.41","temperatura":"52.00","voltagem":"3.32","data_sinal":"2024-11-27 15:22:00","id_perfil":"1","slotno":"1","ponno":"5","tipo_autenticacao":"MAC","versao":"","vlan":"1000","causa_ultima_queda":"LOSi\/LOBi","onu_rede_neutra":"N","id_hardware":"0"},{"id_projeto":"26","gemport":"","ip_gerencia":"","login_onu_cliente":"","senha_onu_cliente":"","porta_telnet_onu_cliente":"","perfil_onu_cliente":"0","script_onu_cliente":"","senorid":"","latitude":"-20.123450633114","longitude":"-40.322125313527","endereco_padrao_cliente":"S","id_condominio":"0","bloco":"","apartamento":"0","cep":"","endereco":"","numero":"","bairro":"","cidade":"0","referencia":"","complemento":"","distancia_onu":"1473","vlan_dhcp":"","vlan_tr69":"","vlan_iptv":"","vlan_voip":"","vlan_pppoe":"","vlan_outros":"","id_ramal":"0","id_onu_unms":"","id_activity":"","radpop_estrutura":"N","porta_web_onu_cliente":"0","tipo_operacao":"","id":"7314","id_transmissor":"3","nome":"12707_BRUNO_ORTEGA_BLANES\r","id_caixa_ftth":"16","porta_ftth":"13","id_login":"9171","id_contrato":"10085","onu_numero":"47","service_port":"","onu_tipo":"","ponid":"0\/1\/5","comandos":"interface gpon #subrack#\/#slot#; ont add #pon# sn-auth #onu_mac# omci ont-lineprofile-id #vlan# ont-srvprofile-id 200 desc #nome#; ont port native-vlan #pon# #onu_numero# eth 1 vlan #vlan#; quit; service-port vlan #vlan# gpon #pon_id# ont #onu_numero# gemport 10 multi-service user-vlan #vlan#;","mac":"4857544376C0E3A5","sinal_rx":"21.35","sinal_tx":"25.41","temperatura":"52.00","voltagem":"3.32","data_sinal":"2024-11-27 15:22:00","id_perfil":"1","slotno":"1","ponno":"5","tipo_autenticacao":"MAC","versao":"","vlan":"1000","causa_ultima_queda":"LOSi\/LOBi","onu_rede_neutra":"N","id_hardware":"0"}]}""";

    internal const string FiberClient_Not_Found = """{"page":"1","total":"0"}""";

    internal const string FiberClient_Broken_Response = """{"page":"1","total":"1"}""";

    internal const string FiberClient_Broken_Json = """{"page":"1","total":""";
}
