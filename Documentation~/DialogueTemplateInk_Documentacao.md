# Dialogue Template Ink

Sistema de Diálogo para Unity baseado em Ink

**Versão:** 1.0

---

## Índice

1. [Visão Geral](#1-visão-geral)
2. [Requisitos](#2-requisitos)
3. [Estrutura de Arquivos](#3-estrutura-de-arquivos)
4. [Configuração do Projeto](#4-configuração-do-projeto-passo-a-passo)
5. [Construindo a Interface (UI)](#5-construindo-a-interface-ui)
6. [Configurando o Componente no Inspector](#6-configurando-o-componente-no-inspector)
7. [Editor Customizado (Opcional)](#7-editor-customizado-opcional)
8. [Como Iniciar um Diálogo via Script](#8-como-iniciar-um-diálogo-via-script)
9. [Eventos Disponíveis](#9-eventos-disponíveis)
10. [Customização](#10-customização-typewriter-skip-módulos)
11. [Solução de Problemas](#11-solução-de-problemas-troubleshooting)
12. [Notas Finais](#12-notas-finais)

---

## 1. Visão Geral

O **DialogueTemplateInk** é um sistema de diálogo para Unity construído sobre o [Ink](https://www.inklestudios.com/ink/) (inkle's narrative scripting language). Ele oferece:

- Diálogo simples (linha única, com efeito de máquina de escrever)
- Diálogo com escolhas (*branching dialogue*)
- Efeito de typewriter configurável, com pausas em pontuação
- Sistema de skip (pular texto sendo digitado)
- **Módulos independentes**: use só diálogo simples, só com escolhas, ou ambos ao mesmo tempo
- Evento estático disparado ao iniciar um diálogo, útil para integrações (pausar o jogador, abrir HUD, tocar som, etc.)

> O script principal é um **Singleton** (`DialogueTemplateInk.Instance`), então pode ser chamado de qualquer lugar do projeto sem precisar de referência direta.

---

## 2. Requisitos

| Requisito | Observação |
|---|---|
| Unity 2021.3 LTS ou superior | Recomendado |
| TextMeshPro (TMP) | Importado no projeto |
| Ink Unity Integration | Pacote oficial da inkle |
| Arquivo `.ink` compilado em `.json` | Sua história, como `TextAsset` |

---

## 3. Estrutura de Arquivos

```
Assets/
├── Scripts/
│   └── DialogueSystem/
│       └── DialogueTemplateInk.cs        # script principal, runtime
│
├── Editor/
│   └── DialogueSystem/
│       └── DialogueTemplateInkEditor.cs  # script do editor (OPCIONAL)
│
└── Pasta de Diálogos/
    └── SuaHistoria.ink / SuaHistoria.json
```

> ⚠️ **Importante:** o script de Editor precisa estar dentro de uma pasta chamada exatamente `Editor` (em qualquer nível dentro de `Assets`). O Unity reconhece esse nome automaticamente e exclui o conteúdo do build final.

---

## 4. Configuração do Projeto (Passo a Passo)

### Passo 1 — Instalar o Ink Unity Integration
`Window > Package Manager` → pesquise por **"Ink"**, ou importe via `.unitypackage` disponível no GitHub oficial da inkle.

### Passo 2 — Importar o TextMeshPro
`Window > TextMeshPro > Import TMP Essential Resources`
(caso ainda não tenha feito isso no projeto)

### Passo 3 — Adicionar os scripts ao projeto
- Copie `DialogueTemplateInk.cs` para `Assets/Scripts/DialogueSystem/`
- *(Opcional)* Copie `DialogueTemplateInkEditor.cs` para `Assets/Editor/DialogueSystem/`

### Passo 4 — Escrever e compilar sua história em Ink
Crie um arquivo `.ink`, escreva sua narrativa, e o Unity gera automaticamente um `.json` correspondente ao salvar (graças ao plugin Ink).
Recomenda-se a utilização do seguinte tutorial para se aprender a programar uma história em Ink: [
Learn Ink (video game dialogue language) in 15 minutes | Ink tutorial](https://youtu.be/KSRpcftVyKg?si=ahFavkCJfLvz1Wm3)

---

## 5. Construindo a Interface (UI)

> O template **não inclui prefabs prontos** propositalmente — a ideia é que você monte a interface com a identidade visual do seu próprio jogo.

### Estrutura sugerida no Canvas

```
Canvas
└── DialoguePanel
    ├── SimpleDialogueBox           → vincular em "Simple Text Box"
    │   ├── Text (TMP)              → vincular em "Simple Text"
    │   └── ContinueButton (Button) → vincular em "Continue Button"
    │
    └── ChoiceDialogueBox           → vincular em "Choice Text Box"
        ├── Text (TMP)              → vincular em "Choice Text"
        ├── ContinueButton (Button) → vincular em "Choice Continue Button"
        └── Scroll View
            └── Viewport                        
                └── Content         → vincular em "Button Container"
```
### Prefab de botão de escolha (Button Prefab)

Crie um prefab simples contendo:
- `Button` (componente Unity UI)
- `TextMeshProUGUI` como filho (para exibir o texto da escolha)

### Configuração do Scroll View (Button Container)

O prefab criado será instanciado dinamicamente dentro do **Button Container** a cada escolha disponível na história.
Para melhor organização é recomendado desabilitar a movimentação horizontal ou vertical em **Scroll Rect** dependendendo de como os botões devem ser dispostos.

> 💡 **Dicas de layout:** adicione um `Vertical Layout Group` + `Content Size Fitter` no Button Container e os configure ao próprio gosto, para que os botões de escolha se organizem automaticamente conforme a quantidade de opções. Adicione também um `Layout Element` ao Button Prefab e o configure ao seu próprio gosto para manter o visual idealizado do botão.

---

## 6. Configurando o Componente no Inspector

Adicione o script `DialogueTemplateInk` a um GameObject vazio na cena (ex: `DialogueManager`) e preencha os campos:

**Módulos Ativos**
- `Use Simple Dialogue` — habilita/desabilita o módulo de diálogo simples
- `Use Choice Dialogue` — habilita/desabilita o módulo de escolhas

**Simple Dialogue**
- `Simple Text Box` — GameObject do painel de diálogo simples
- `Simple Text` — componente TextMeshProUGUI do texto
- `Continue Button` — botão de avançar o diálogo

**Choice Dialogue**
- `Choice Text Box` — GameObject do painel de escolhas
- `Button Prefab` — prefab do botão de escolha
- `Choice Text` — componente TextMeshProUGUI do texto da escolha
- `Button Container` — Transform onde os botões serão instanciados
- `Choice Continue Button` — botão de avançar o diálogo, ainda requer que um botão de escolha seja pressionado

**Typewriter**
- `Characteres Per Second` — velocidade da digitação (caracteres/segundo)
- `Interpunctuation Delay` — pausa extra ao encontrar pontuação (`. , ! ? : ;`)

> ℹ️ Se você desativar um módulo (ex: `Use Choice Dialogue = false`), não é obrigatório preencher os campos daquele módulo. O script possui proteções internas (*null checks*) para evitar erros de referência.

---

## 7. Editor Customizado (Opcional)

O arquivo `DialogueTemplateInkEditor.cs` é **opcional** e serve apenas para melhorar a organização visual do Inspector, agrupando os campos em seções recolhíveis (*foldouts*) e desabilitando visualmente os campos de um módulo quando ele está desligado.

**Se você não usar o editor:**
O componente funciona normalmente. Os campos aparecerão na ordem padrão do Unity, com os bools `Use Simple Dialogue` e `Use Choice Dialogue` como checkboxes comuns no topo do Inspector.

**Se você usar o editor:**
Copie o arquivo para uma pasta chamada `Editor` dentro de `Assets`. O Inspector passará a exibir três seções recolhíveis:
- Simple Dialogue
- Choice Dialogue
- Typewriter

Os campos de cada módulo ficam acinzentados (desabilitados) quando o respectivo `Use...` está desmarcado, evitando preenchimento desnecessário.

> ⚠️ Caso renomeie alguma variável serializada no script principal, atualize a referência correspondente em `serializedObject.FindProperty("nomeDoCampo")` no script do editor.

---

## 8. Como Iniciar um Diálogo via Script

Antes de vincular o sistema de diálogos ao seu código não se esqueça de adicionar o namespace ao ínicio.

```csharp
using DialogueSystem;
```

Para iniciar uma conversa, basta chamar o método `StartDialogue`, passando o `TextAsset` (`.json`) gerado pelo Ink:

```csharp
public TextAsset minhaHistoria;

void Interagir()
{
    DialogueTemplateInk.Instance.StartDialogue(minhaHistoria);
}
```

Para verificar se um diálogo está em andamento (por exemplo, para travar o movimento do jogador):

```csharp
if (DialogueTemplateInk.Instance.IsDialogueActive)
{
    // bloquear input do jogador
}
```

---

## 9. Eventos Disponíveis

### `DialogueStarted` (`Action<Story, TextAsset>`)

Disparado sempre que um novo diálogo é iniciado (`StartDialogue`). Útil para integrações externas, como:
- Vincular variáveis externas à `Story` do Ink
- Pausar o jogo
- Tocar uma música de fundo específica da cena de diálogo

**Exemplo de inscrição:**

```csharp
void OnEnable()
{
    DialogueTemplateInk.DialogueStarted += HandleDialogueStarted;
}

void OnDisable()
{
    DialogueTemplateInk.DialogueStarted -= HandleDialogueStarted;
}

void HandleDialogueStarted(Story story, TextAsset asset)
{
    Debug.Log("Diálogo iniciado: " + asset.name);
}
```

---

## 10. Customização (Typewriter, Skip, Módulos)

**Velocidade do typewriter**
Ajuste `Characteres Per Second` no Inspector. Valores maiores tornam o texto mais rápido.

**Pausa em pontuação**
`Interpunctuation Delay` define quanto tempo extra (em segundos) o sistema espera ao encontrar os caracteres `. , ! ? : ;`

**Pular texto (skip)**
O método `ContinueButton` já trata isso automaticamente: se o texto ainda está sendo digitado, o primeiro clique completa o texto instantaneamente; o segundo clique avança a história.

**Usar apenas um módulo**
Caso seu jogo use apenas diálogos lineares (sem escolhas), desmarque `Use Choice Dialogue` e deixe os campos daquele módulo vazios. O mesmo vale para o inverso, caso queira um sistema só de escolhas.

---

## 11. Solução de Problemas (Troubleshooting)

| Problema | Solução |
|---|---|
| `NullReferenceException` ao chamar `StartDialogue` | Verifique se o `Continue Button` foi vinculado no Inspector, mesmo que você use apenas o módulo de escolhas (o listener é adicionado no `Start` independente do módulo ativo) |
| Os botões de escolha não aparecem | Confirme que o `Button Container` possui um Layout Group e que o `Button Prefab` contém um `TextMeshProUGUI` como filho direto ou descendente |
| O texto não aparece, mas o diálogo "avança" sozinho | Verifique se o TextMeshProUGUI vinculado em `Simple Text` ou `Choice Text` está ativo e não possui Canvas Group com alpha 0 ou similar |
| O Editor customizado não aparece | Confirme que o arquivo está dentro de uma pasta chamada exatamente `Editor` e que não há erros de compilação no Console |

---

## 12. Notas Finais

Este template foi pensado para ser **modular e extensível**. Sinta-se livre para adicionar novos módulos (ex: diálogo com retrato de personagem, diálogo com áudio/voz, tags do Ink para emoções, etc.) seguindo o mesmo padrão: bool de ativação + null checks + seção própria no editor.

Prefabs de UI não são fornecidos propositalmente, garantindo que a identidade visual do diálogo siga o estilo artístico do projeto onde o template for aplicado.

---

*Fim do documento*
