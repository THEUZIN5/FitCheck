# Novos cachorros e celebração da semana — 14/11/2026

Pinscher, Shih-tzu e Golden Retriever adicionados como IDs 3/4/5. Vira-lata, Shiba e Dachshund mantêm IDs 0/1/2; gatos, pelagens, nome, respiração, expressão, carinho e persistência por save preservados. O criador acomoda as seis raças sem sobrepor Voltar/Salvar.

Pinscher pequeno com orelhas erguidas e marcas caramelo; Shih-tzu com focinho curto, barba, orelhas peludas e cauda pompom; Golden com porte maior, orelhas caídas e volume de pelagem no peito e laterais. Construção leve usa as malhas e materiais do sistema de pets existente; máximo medido de 4.847 vértices por cachorro nas 36 combinações.

Ao receber as recompensas e fechar o último dia dos 21 encontros, uma sequência de 7,4 s reúne jogador e clientes diante dos fogos. Pode pausar ou avançar para a foto; terminar o tempo abre automaticamente a lembrança. A câmera é restaurada e a foto segue guardada no quarto. Reabrir uma foto existente não repete a sequência nem a sobrescreve.

Fogos temporários: uma malha dinâmica, um material reutilizado, cinco explosões com rastros, nenhuma luz em tempo real e nenhum ator permanente novo. Movimento reduzido usa três constelações estáticas com aparecimento/desaparecimento suave e câmera fixa. Não há clarão de tela inteira. Pausa/foco e mute preservados; o disparo da foto usa o som já existente. Não foi acrescentado um som específico de explosão.

Validação nativa `-city-proof -pets-finale-review`: 36 combinações de raça/pelagem salvas e recarregadas, IDs legados, orçamento de malhas, pausa, movimento reduzido, campanha completa, avanço manual e automático, restauração da câmera e preservação da foto/progresso. Resultado `PETS_FINALE_PASSED`. Compilação de revisão concluída sem erros ou avisos.

Prévia real em `Captures/PetsFinale/`: `novos-cachorros.jpg`, `dog-3.png`, `dog-4.png`, `dog-5.png`, `finale-fireworks.png`, `finale-reduced.png`. Dados usados na conferência são isolados. As builds Windows/Web publicáveis continuam sendo as entregues em 13/11; incluir estas mudanças na próxima build solicitada.

Para reconstruir a revisão: método Editor `FitCheck.Editor.PetsFinaleReview.Build`; executar a build temporária com os argumentos acima e `-proof-output` apontando para `Captures/PetsFinale` (onde o método grava as soluções). Nunca executar a conferência usando dados reais do jogador.
